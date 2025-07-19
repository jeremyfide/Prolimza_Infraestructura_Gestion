
document.addEventListener("DOMContentLoaded", function () {
    const hoy = new Date();
    const haceUnMes = new Date();
    haceUnMes.setDate(haceUnMes.getDate() - 30);

    document.getElementById("fecha-inicio").value = "2025-06-19";
    document.getElementById("fecha-fin").value = "2025-07-19";

    const desde = document.getElementById("fecha-inicio").value;
    const hasta = new Date(document.getElementById("fecha-fin").value);
    hasta.setHours(23, 59, 59, 999);
    const hastaISO = hasta.toISOString();

	const tipoProducto = document.getElementById("tipoProducto").value || "";

	cargarTablaProduccion(desde, hastaISO, tipoProducto);
	cargarPieChartRecetas(desde, hastaISO, tipoProducto);
	cargarStackChartMateriaPrima(desde, hastaISO, tipoProducto);
	cargarGraficoEficienciaMateriaPrima(desde, hastaISO, tipoProducto);
});

document.getElementById("btn-filtrar").addEventListener("click", () => {
    const desde = document.getElementById("fecha-inicio").value;
    const hasta = document.getElementById("fecha-fin").value;

    if (!desde || !hasta) {
        alert("Debes seleccionar ambas fechas.");
        return;
    }

    const fechaFinCompleta = new Date(hasta);
    fechaFinCompleta.setHours(23, 59, 59, 999);
    const fechaFinISO = fechaFinCompleta.toISOString();

	const tipoProducto = document.getElementById("tipoProducto").value || "";

	cargarTablaProduccion(desde, fechaFinISO, tipoProducto);
	cargarPieChartRecetas(desde, fechaFinISO, tipoProducto);
	cargarStackChartMateriaPrima(desde, fechaFinISO, tipoProducto);
	cargarGraficoEficienciaMateriaPrima(desde, fechaFinISO, tipoProducto);
});

async function cargarTablaProduccion(fechaInicio, fechaFin, tipoProducto) {
	try {
		tipoProducto = tipoProducto || "";

		const url = `/api/produccionapi/reporte-produccion?fechaInicio=${fechaInicio}&fechaFin=${fechaFin}&tipoProducto=${tipoProducto}`;

		const response = await fetch(url);

		if (!response.ok) throw new Error(`Error en respuesta: ${response.status}`);

		const rawData = await response.json();
		const data = Array.isArray(rawData) ? rawData : (rawData?.$values ?? []);

		if ($.fn.DataTable.isDataTable('#miTabla')) {
			$('#miTabla').DataTable().clear().destroy();
		}

		const tbody = document.querySelector("#miTabla tbody");
		tbody.innerHTML = "";

		data.forEach(item => {
			const row = document.createElement("tr");
			row.innerHTML = `
				<td>${new Date(item.fechaVenta).toISOString().split("T")[0]}</td>
				<td>${item.nombreProducto}</td>
				<td>${item.tipoProducto}</td>
				<td>${item.nombreMateriaPrima ?? "-"}</td>
				<td>${item.cantidadMateriaPrimaUsada?.toFixed(2) ?? "-"}</td>
				<td>${item.cantidadProductoVendido}</td>`;
			tbody.appendChild(row);
		});

        $('#miTabla').DataTable({
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'copyHtml5',
                    text: 'Copiar',
                    exportOptions: { columns: ':visible' }
                },
                {
                    extend: 'excelHtml5',
                    text: 'Exportar a Excel',
                    exportOptions: { columns: ':visible' }
                },
                {
                    extend: 'pdfHtml5',
                    text: 'Exportar a PDF',
                    exportOptions: { columns: ':visible' }
                },
                {
                    extend: 'print',
                    text: 'Imprimir',
                    exportOptions: { columns: ':visible' }
                }
            ],
            language: {
                decimal: ",",
                thousands: ".",
                search: "Buscar:",
                lengthMenu: "Mostrar _MENU_ registros",
                info: "Mostrando _START_ a _END_ de _TOTAL_ registros",
                infoEmpty: "Mostrando 0 a 0 de 0 registros",
                infoFiltered: "(filtrado de _MAX_ registros totales)",
                loadingRecords: "Cargando...",
                processing: "Procesando...",
                zeroRecords: "No se encontraron resultados",
                emptyTable: "No hay datos disponibles en la tabla",
                paginate: {
                    first: "Primero",
                    previous: "Anterior",
                    next: "Siguiente",
                    last: "Último"
                },
                aria: {
                    sortAscending: ": activar para ordenar ascendente",
                    sortDescending: ": activar para ordenar descendente"
                },
                buttons: {
                    copy: "Copiar",
                    excel: "Excel",
                    pdf: "PDF",
                    print: "Imprimir"
                }
            },
            paging: true,
            searching: true,
            ordering: true,
            info: true,
            lengthChange: false,
            pageLength: 15
        });

	} catch (error) {
		alert("Hubo un error al cargar los datos del servidor.");
	}
}

async function cargarPieChartRecetas(fechaInicio, fechaFin, tipoProducto) {
	const url = `/api/produccionapi/recetas-mas-utilizadas?fechaInicio=${fechaInicio}&fechaFin=${fechaFin}&tipoProducto=${tipoProducto}`;
	const response = await fetch(url);
	const rawData = await response.json();
	const data = Array.isArray(rawData) ? rawData : (rawData?.$values ?? []);


	const nombres = data.map(r => r.receta);
	const cantidades = data.map(r => r.total);

	const chartElement = document.querySelector("#pie-chart");
	if (!chartElement) return;

	if (window.pieChartInstance) {
		window.pieChartInstance.destroy();
	}

	window.pieChartInstance = new ApexCharts(chartElement, {
		series: cantidades,
		labels: nombres,
		chart: { type: 'donut', height: 350 },
		tooltip: {
			y: { formatter: val => `${val} usos` }
		}
	});

	window.pieChartInstance.render();
}

async function cargarStackChartMateriaPrima(fechaInicio, fechaFin, tipoProducto) {
	const url = `/api/produccionapi/materia-por-dia?fechaInicio=${fechaInicio}&fechaFin=${fechaFin}&tipoProducto=${tipoProducto}`;
	const response = await fetch(url);
	const rawData = await response.json();
	const data = Array.isArray(rawData) ? rawData : (rawData?.$values ?? []);


	const fechas = [...new Set(data.map(d => d.fecha))];
	const materias = [...new Set(data.map(d => d.materia))];

	const series = materias.map(materia => ({
		name: materia,
		data: fechas.map(fecha => {
			const match = data.find(d => d.fecha === fecha && d.materia === materia);
			return match ? match.total : 0;
		})
	}));

	const chartElement = document.querySelector("#column-chart");
	if (!chartElement) return;

	if (window.columnChartInstance) {
		window.columnChartInstance.destroy();
	}

	window.columnChartInstance = new ApexCharts(chartElement, {
		series: series,
		chart: {
			type: 'bar',
			height: 350,
			stacked: true,
			toolbar: { show: false }
		},
		xaxis: {
			categories: fechas,
			labels: { rotate: -45 }
		},
		legend: { position: 'top' },
		tooltip: {
			y: { formatter: val => `${val} unidades` }
		}
	});

	window.columnChartInstance.render();
}

async function cargarGraficoEficienciaMateriaPrima(fechaInicio, fechaFin) {
    const tipoProducto = document.getElementById("tipoProducto").value || "";
    const url = `/api/produccionapi/eficiencia-materia-prima?fechaInicio=${encodeURIComponent(fechaInicio)}&fechaFin=${encodeURIComponent(fechaFin)}&tipoProducto=${encodeURIComponent(tipoProducto)}`;

    const response = await fetch(url);
    const rawData = await response.json();
    const data = Array.isArray(rawData) ? rawData : (rawData?.$values ?? []);

    if (data.length === 0) {
        console.warn("No hay datos de eficiencia por materia prima.");
        return;
    }

    const fechas = data.map(d => d.fecha);
    const materiasSet = new Set();

    // Recolectamos todos los nombres de materias primas únicos
    data.forEach(entry => {
        const detalle = Array.isArray(entry.detalle) ? entry.detalle : (entry.detalle?.$values ?? []);
        detalle.forEach(m => materiasSet.add(m.materia));
    });

    const materias = Array.from(materiasSet);

    // Armamos las series: cada materia tendrá un array con su eficiencia por fecha
    const series = materias.map(nombreMateria => ({
        name: nombreMateria,
        data: fechas.map(fecha => {
            const dia = data.find(d => d.fecha === fecha);
            const detalle = Array.isArray(dia.detalle) ? dia.detalle : (dia.detalle?.$values ?? []);
            const match = detalle.find(d => d.materia === nombreMateria);
            return match ? match.eficiencia : 0;
        })
    }));

    // Renderizado con ApexCharts
    const chartElement = document.querySelector("#line-chart");
    if (!chartElement) return;

    if (window.eficienciaMateriaChartInstance) {
        window.eficienciaMateriaChartInstance.destroy();
    }

    window.eficienciaMateriaChartInstance = new ApexCharts(chartElement, {
        series: series,
        chart: {
            type: 'line',
            height: 400,
            toolbar: { show: true }
        },
        xaxis: {
            categories: fechas,
            title: { text: 'Fecha' },
            labels: { rotate: -45 }
        },
        yaxis: {
            title: { text: 'Eficiencia Relativa (%)' },
            max: 100
        },
        tooltip: {
            shared: true,
            y: {
                formatter: val => `${val.toFixed(2)}%`
            }
        },
        stroke: {
            width: 2,
            curve: 'smooth'
        },
        markers: {
            size: 4
        },
        legend: {
            position: 'top'
        }
    });

    window.eficienciaMateriaChartInstance.render();
}






/*
document.getElementById("descargar-pdf").addEventListener("click", async () => {
	const { jsPDF } = window.jspdf;
	const pdf = new jsPDF("p", "mm", "a4");
	let y = 10;

	const graficos = ["column-chart", "line-chart", "pie-chart", "stack-chart"];

	function buscarTituloGrafico(chartEl) {
		let parent = chartEl.parentElement;
		while (parent) {
			const titulo = parent.querySelector(".geex-content__section__header__title");
			if (titulo && titulo.textContent.trim()) {
				return titulo.textContent.trim();
			}
			parent = parent.parentElement;
		}
		return "gráfico";
	}

	for (let i = 0; i < graficos.length; i++) {
		const id = graficos[i];
		const chartEl = document.getElementById(id);
		if (!chartEl) continue;

		const titulo = buscarTituloGrafico(chartEl);

		pdf.setFontSize(14);
		pdf.text(titulo, 10, y);
		y += 6;

		await html2canvas(chartEl, { scale: 2 }).then(canvas => {
			const imgData = canvas.toDataURL("image/png");
			const imgProps = pdf.getImageProperties(imgData);
			const pdfWidth = 160;
			const pdfHeight = 80;

			if (y + pdfHeight > 290) {
				pdf.addPage();
				y = 10;
			}

			pdf.addImage(imgData, "PNG", 10, y, pdfWidth, pdfHeight);
			y += pdfHeight + 10;
		});
	}

	const pageTitle = document.querySelector(".geex-content__header__title")?.textContent.trim().replace(/\s+/g, "_") || "reporte";

	const fecha = new Date();
	const yyyy = fecha.getFullYear();
	const mm = String(fecha.getMonth() + 1).padStart(2, "0");
	const dd = String(fecha.getDate()).padStart(2, "0");
	const hh = String(fecha.getHours()).padStart(2, "0");
	const min = String(fecha.getMinutes()).padStart(2, "0");
	const ss = String(fecha.getSeconds()).padStart(2, "0");
	const fechaHora = `${yyyy}-${mm}-${dd}_${hh}-${min}-${ss}`;

	pdf.save(`${pageTitle}_${fechaHora}.pdf`);
});
*/