(function ($) {

	window.addEventListener("DOMContentLoaded", () => {
		const hoy = new Date();
		const haceUnMes = new Date();
		haceUnMes.setDate(haceUnMes.getDate() - 30);

		document.getElementById("fecha-inicio").value = haceUnMes.toISOString().split("T")[0];
		document.getElementById("fecha-fin").value = hoy.toISOString().split("T")[0];

		// Cargar gráficos inicialmente
		const desde = document.getElementById("fecha-inicio").value;
		const hasta = new Date(document.getElementById("fecha-fin").value);
		hasta.setHours(23, 59, 59, 999);
		const hastaISO = hasta.toISOString();

		cargarGraficoVentasPorUsuario(desde, hastaISO);
		cargarDatosBarChartVentas(desde, hastaISO);
		cargarPieChartProductos(desde, hastaISO);
		cargarStackChartVentas(desde, hastaISO);
	});

	document.getElementById("btn-filtrar").addEventListener("click", () => {
		const desde = document.getElementById("fecha-inicio").value;
		const hasta = document.getElementById("fecha-fin").value;

		if (!desde || !hasta) {
			alert("Debes seleccionar ambas fechas.");
			return;
		}

		// Ajustar la fecha final para incluir todo el día (23:59:59)
		const fechaFinCompleta = new Date(hasta);
		fechaFinCompleta.setHours(23, 59, 59, 999);

		const fechaFinISO = fechaFinCompleta.toISOString();

		// Recargar todos los gráficos con estas fechas
		cargarGraficoVentasPorUsuario(desde, fechaFinISO);
		cargarDatosBarChartVentas(desde, fechaFinISO);
		cargarPieChartProductos(desde, fechaFinISO);
		cargarStackChartVentas(desde, fechaFinISO);
	});


	//Graficos ventas
	//Ventas por semana
	async function cargarDatosBarChartVentas(fechaInicio, fechaFin) {
		const url = `/api/ventasapi/ventas-por-semana?fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`;

		const response = await fetch(url);
		if (!response.ok) {
			const text = await response.text();
			console.error("Error en la respuesta:", text);
			return;
		}

		const rawData = await response.json();
		const data = rawData.$values ?? [];

		if (!Array.isArray(data)) {
			console.error("La respuesta no es un arreglo:", data);
			return;
		}

		const semanas = data.map(d => "Semana " + (d.semanaInicio || d.semana));
		const valores = data.map(d => d.totalVentas);

		const totalVentas = valores.reduce((a, b) => a + b, 0);

		document.getElementById("ventas-totales").textContent = totalVentas.toLocaleString("es-CR");

		const chartElement = document.querySelector("#column-chart");
		if (!chartElement) return;

		if (window.barChartInstance) {
			window.barChartInstance.destroy();
		}

		window.barChartInstance = new ApexCharts(chartElement, {
			series: [{ data: valores }],
			chart: { height: 250, type: 'bar', toolbar: { show: false } },
			colors: ["#4DAA5726"],
			plotOptions: {
				bar: {
					columnWidth: '40%',
					borderRadius: 12,
				}
			},
			dataLabels: { enabled: false },
			xaxis: {
				categories: semanas,
				labels: {
					rotate: -45,
					style: { fontSize: '11px' }
				}
			},
			yaxis: {
				labels: { show: true },
			},
			grid: { show: false },
			tooltip: {
				enabled: true,
				custom: function ({ series, seriesIndex, dataPointIndex, w }) {
					let value = w.globals.series[seriesIndex][dataPointIndex];
					let max = Math.max(...series[seriesIndex]);
					let percentage = ((value / max) * 100).toFixed(0);
					return `
					<div class="custom-tooltip">
						<span class="custom-tooltip__title">${percentage}%</span>
						<span class="custom-tooltip__subtitle">${value} ventas</span>
					</div>`;
				}
			}
		});

		window.barChartInstance.render();
	}


	//Ventas por usuario
	async function cargarGraficoVentasPorUsuario(fechaInicio, fechaFin) {
		const url = `/api/ventasapi/ventas-por-semana-por-usuario?fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`;

		const response = await fetch(url);
		if (!response.ok) {
			const text = await response.text();
			console.error("Error en la respuesta:", text);
			return;
		}

		const rawData = await response.json();
		const data = rawData.$values ?? rawData;

		if (!Array.isArray(data)) {
			console.error("La respuesta no es un arreglo:", data);
			return;
		}

		const semanas = data.map(d => `Semana ${d.semanaInicio ?? d.semana}`);

		const usuariosSet = new Set();
		data.forEach(semana => {
			const usuarios = semana.usuarios?.$values ?? semana.usuarios ?? [];
			usuarios.forEach(u => usuariosSet.add(u.nombre));
		});
		const usuarios = Array.from(usuariosSet);

		const series = usuarios.map(nombre => ({
			name: nombre,
			data: data.map(semana => {
				const usuarios = semana.usuarios?.$values ?? semana.usuarios ?? [];
				const usuario = usuarios.find(u => u.nombre === nombre);
				return usuario ? usuario.totalVentas : 0;
			})
		}));

		const chartElement = document.querySelector("#line-chart");
		if (!chartElement) return;

		if (window.lineChartInstance) {
			window.lineChartInstance.destroy();
		}

		window.lineChartInstance = new ApexCharts(chartElement, {
			series: series,
			chart: {
				height: 350,
				type: 'line',
				toolbar: { show: true }
			},
			xaxis: {
				categories: semanas,
				labels: {
					rotate: -45,
					style: { fontSize: '11px' }
				}
			},
			stroke: {
				width: 2,
				curve: 'smooth'
			},
			markers: {
				size: 4
			},
			tooltip: {
				shared: true,
				intersect: false
			},
			legend: {
				position: 'top'
			}
		});

		window.lineChartInstance.render();
	}


	// Productos por ventas
	// Pie - Ventas por producto
	async function cargarPieChartProductos(fechaInicio, fechaFin) {
		const url = `/api/ventasapi/ventas-por-producto?fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`;

		const response = await fetch(url);
		if (!response.ok) {
			const text = await response.text();
			console.error("Error en la respuesta:", text);
			return;
		}

		const rawData = await response.json();
		const data = rawData.$values ?? rawData;

		if (!Array.isArray(data)) {
			console.error("La respuesta no es un arreglo:", data);
			return;
		}

		const productos = data.map(p => p.producto);
		const cantidades = data.map(p => p.total);

		const chartElement = document.querySelector("#pie-chart");
		if (!chartElement) return;

		if (window.pieChartInstance) {
			window.pieChartInstance.destroy();
		}

		window.pieChartInstance = new ApexCharts(chartElement, {
			series: cantidades,
			labels: productos,
			chart: {
				type: 'donut',
				height: '350px'
			},
			colors: ['#00ADA3', '#374C98', '#FFBB54', '#FF5B5B', '#23292F', '#A0C878'],
			legend: {
				show: true,
				position: 'bottom',
				fontSize: '13px'
			},
			tooltip: {
				y: {
					formatter: function (val) {
						return `${val} ventas`;
					}
				}
			}
		});

		window.pieChartInstance.render();
	}


	// Stack Chart - Cantidad de productos vendidos por usuario
	async function cargarStackChartVentas(fechaInicio, fechaFin) {
		const url = `/api/ventasapi/ventas-por-usuario-producto?fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`;

		const response = await fetch(url);
		if (!response.ok) {
			const text = await response.text();
			console.error("Error en la respuesta:", text);
			return;
		}

		const rawData = await response.json();
		const data = rawData.$values ?? rawData;

		if (!Array.isArray(data)) {
			console.error("La respuesta no es un arreglo:", data);
			return;
		}

		const usuarios = [...new Set(data.map(d => d.usuario))];
		const productos = [...new Set(data.map(d => d.producto))];

		const series = productos.map(producto => ({
			name: producto,
			data: usuarios.map(usuario => {
				const match = data.find(d => d.usuario === usuario && d.producto === producto);
				return match ? match.total : 0;
			})
		}));

		const chartElement = document.querySelector("#stack-chart");
		if (!chartElement) return;

		if (window.stackChartInstance) {
			window.stackChartInstance.destroy();
		}

		window.stackChartInstance = new ApexCharts(chartElement, {
			series: series,
			chart: {
				type: 'bar',
				height: 350,
				stacked: true,
				toolbar: { show: false }
			},
			xaxis: {
				categories: usuarios,
				labels: {
					rotate: -45,
					style: { fontSize: '11px' }
				}
			},
			legend: {
				position: 'top'
			},
			tooltip: {
				y: {
					formatter: val => `${val} unidades`
				}
			},
			plotOptions: {
				bar: {
					horizontal: false,
					columnWidth: '50%',
					borderRadius: 4
				}
			}
		});

		window.stackChartInstance.render();
	}


})(jQuery);