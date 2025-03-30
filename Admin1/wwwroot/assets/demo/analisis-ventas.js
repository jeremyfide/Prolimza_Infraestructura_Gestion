const ctxVentasTendencia = document.getElementById("graficoTendenciaVentas").getContext("2d");

const dataVentasPorProducto = {
    labels: ["Enero", "Febrero", "Marzo", "Abril", "Mayo"],
    datasets: [
        {
            label: "Limpiador Multiusos",
            borderColor: "#0d6efd",
            fill: false,
            data: [120, 135, 150, 160, 180]
        },
        {
            label: "Desinfectante Floral",
            borderColor: "#198754",
            fill: false,
            data: [90, 100, 110, 105, 120]
        },
        {
            label: "Jabón líquido",
            borderColor: "#ffc107",
            fill: false,
            data: [60, 65, 80, 90, 100]
        }
    ]
};

const graficoTendenciaVentas = new Chart(ctxVentasTendencia, {
    type: 'line',
    data: JSON.parse(JSON.stringify(dataVentasPorProducto)),
    options: {
        responsive: true,
        scales: {
            xAxes: [{ gridLines: { display: false } }],
            yAxes: [{ ticks: { beginAtZero: true }, gridLines: { display: true } }]
        },
        legend: { position: 'bottom' }
    }
});

function filtrarTendenciaVentas() {
    const filtro = document.getElementById("filtroProductoVentasMes").value;
    const nuevaData = { labels: dataVentasPorProducto.labels, datasets: [] };

    if (filtro === "all") {
        graficoTendenciaVentas.data = JSON.parse(JSON.stringify(dataVentasPorProducto));
    } else {
        const datasetFiltrado = dataVentasPorProducto.datasets.find(ds => ds.label.includes(filtro));
        if (datasetFiltrado) {
            nuevaData.datasets.push(datasetFiltrado);
            graficoTendenciaVentas.data = nuevaData;
        }
    }

    graficoTendenciaVentas.update();
}

const ctxTiempo = document.getElementById("graficoTiempoEntregasCancelaciones").getContext("2d");

new Chart(ctxTiempo, {
    type: 'bar',
    data: {
        labels: ["Entregas", "Cancelaciones"],
        datasets: [{
            label: "Horas promedio",
            backgroundColor: ["#0d6efd", "#dc3545"],
            borderColor: ["#0d6efd", "#dc3545"],
            borderWidth: 1,
            data: [28, 12] // Ejemplo: 28 horas promedio de entrega, 12 horas hasta cancelación
        }]
    },
    options: {
        scales: {
            xAxes: [{ gridLines: { display: false } }],
            yAxes: [{
                ticks: { beginAtZero: true, stepSize: 10 },
                gridLines: { display: true }
            }]
        },
        legend: { display: false }
    }
});

const ctxFrecuencia = document.getElementById("graficoFrecuenciaVentas").getContext("2d");

const dataSemanal = {
    labels: ["Semana 1", "Semana 2", "Semana 3", "Semana 4"],
    datasets: [{
        label: "Ventas",
        backgroundColor: "rgba(13, 110, 253, 0.8)",
        borderColor: "rgba(13, 110, 253, 1)",
        data: [120, 135, 150, 160]
    }]
};

const dataDiaria = {
    labels: ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"],
    datasets: [{
        label: "Ventas",
        backgroundColor: "rgba(13, 110, 253, 0.8)",
        borderColor: "rgba(13, 110, 253, 1)",
        data: [30, 28, 35, 40, 42, 15, 10]
    }]
};

const graficoFrecuenciaVentas = new Chart(ctxFrecuencia, {
    type: 'bar',
    data: JSON.parse(JSON.stringify(dataSemanal)),
    options: {
        scales: {
            yAxes: [{
                ticks: { beginAtZero: true },
                gridLines: { display: true }
            }]
        },
        legend: { display: false }
    }
});

function actualizarGraficoFrecuencia(vista) {
    if (vista === "semana") {
        graficoFrecuenciaVentas.data = JSON.parse(JSON.stringify(dataSemanal));
    } else {
        graficoFrecuenciaVentas.data = JSON.parse(JSON.stringify(dataDiaria));
    }
    graficoFrecuenciaVentas.update();
}