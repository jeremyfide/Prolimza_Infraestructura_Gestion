	(function ($) {
		window.addEventListener("DOMContentLoaded", () => {
			// Cargar ambos gráficos apenas la página esté lista
			cargarDatosBarChartRecetas();
			cargarPieChartTopProductos();
		});

		// 📊 Column chart: Materia prima usada por día (últimos 7 días)
		async function cargarDatosBarChartRecetas() {
			const url = `/InteligenciaNegocio/GetRecetasPorDia`;

			const response = await fetch(url);
			if (!response.ok) {
				console.error("Error al obtener datos de recetas por día");
				return;
			}

			const data = await response.json();

			const lista = data.$values || [];

			const fechas = lista.map(d => new Date(d.fecha).toLocaleDateString());
			const cantidades = lista.map(d => d.cantidadProductosVendidos);


			const chartElement = document.querySelector("#column-chart");
			if (!chartElement) return;

			if (window.barChartInstance) {
				window.barChartInstance.destroy();
			}

			window.barChartInstance = new ApexCharts(chartElement, {
				series: [{ name: "MP usada (kg)", data: cantidades }],
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
					categories: fechas,
					labels: { rotate: -45, style: { fontSize: '11px' } }
				},
				yaxis: {
					labels: { show: true }
				},
				grid: { show: false },
				tooltip: {
					y: {
						formatter: val => `${val.toFixed(2)} kg`
					}
				}
			});

			window.barChartInstance.render();
		}

		// 🥧 Pie chart: Top 3 productos más usados en recetas (últimos 7 días)
		async function cargarPieChartTopProductos() {
			const url = `/InteligenciaNegocio/GetTopProductos`;

			const response = await fetch(url);
			if (!response.ok) {
				console.error("Error al obtener top de productos");
				return;
			}

			const data = await response.json();

			const lista = data.$values || [];

			const productos = lista.map(p => p.nombreProducto);
			const cantidades = lista.map(p => p.cantidadVendida);

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
					height: 350
				},
				colors: ['#00ADA3', '#374C98', '#FFBB54'],
				legend: {
					show: true,
					position: 'bottom',
					fontSize: '13px'
				},
				tooltip: {
					y: {
						formatter: val => `${val} usos`
					}
				}
			});

			window.pieChartInstance.render();
		}
	})(jQuery);
