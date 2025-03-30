// Configuración global para el estilo de Bootstrap
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';

// Datos de ejemplo para el gráfico (se mostrarán cuando no se apliquen filtros)
const datosOriginales = {
    labels: ["Limpiador Multiusos", "Desinfectante Floral", "Jabón líquido"],
    datasets: [{
        label: "Cantidad de veces usado",
        backgroundColor: "rgba(2,117,216,0.8)",
        borderColor: "rgba(2,117,216,1)",
        data: [18, 12, 9]
    }]
};

// Inicializar el gráfico con los datos de ejemplo
const ctx = document.getElementById("graficoRecetas").getContext('2d');
const graficoRecetas = new Chart(ctx, {
    type: 'bar',
    data: JSON.parse(JSON.stringify(datosOriginales)),
    options: {
        scales: {
            xAxes: [{
                gridLines: { display: false },
                ticks: { maxTicksLimit: 6 }
            }],
            yAxes: [{
                ticks: {
                    min: 0,
                    max: 20,
                    stepSize: 5
                },
                gridLines: { display: true }
            }]
        },
        legend: { display: false }
    }
});

// Función para aplicar el filtro (prototipo visual)
function filtrarGraficoRecetas() {
    const productoSeleccionado = document.getElementById("filtroProducto").value;

    // Si "all" está seleccionado, se restauran los datos originales
    if (productoSeleccionado === "all") {
        graficoRecetas.data = JSON.parse(JSON.stringify(datosOriginales));
    } else {
        // Buscar el índice del producto seleccionado dentro de los datos originales
        const index = datosOriginales.labels.findIndex(label => label.includes(productoSeleccionado));
        if (index !== -1) {
            const nuevoData = {
                labels: [datosOriginales.labels[index]],
                datasets: [{
                    label: "Cantidad de veces usado",
                    backgroundColor: datosOriginales.datasets[0].backgroundColor,
                    borderColor: datosOriginales.datasets[0].borderColor,
                    data: [datosOriginales.datasets[0].data[index]]
                }]
            };
            graficoRecetas.data = nuevoData;
        }
    }
    graficoRecetas.update();
}
