Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';

// Datos iniciales (ejemplo)
const datosVentasOriginales = {
    labels: ["Limpiador Multiusos", "Desinfectante Floral", "Jabón líquido"],
datasets: [{
    data: [150, 95, 75],
backgroundColor: ["#007bff", "#28a745", "#ffc107"]
    }]
};

// Crear gráfico de pie
const ctxVentas = document.getElementById("graficoVentas").getContext("2d");
const graficoVentas = new Chart(ctxVentas, {
    type: 'pie',
data: JSON.parse(JSON.stringify(datosVentasOriginales)),
options: {
    responsive: true,
legend: {
    position: 'bottom'
        }
    }
});

// Filtro manual (mock visual)
function filtrarGraficoVentas() {
    const producto = document.getElementById("filtroProductoVentas").value;
const nuevaData = {labels: [], datasets: [{data: [], backgroundColor: [] }] };

const colores = ["#007bff", "#28a745", "#ffc107"];
const labels = datosVentasOriginales.labels;
const valores = datosVentasOriginales.datasets[0].data;

if (producto === "all") {
    // Restaurar original
    graficoVentas.data = JSON.parse(JSON.stringify(datosVentasOriginales));
    } else {
        const index = labels.findIndex(label => label.includes(producto));
if (index !== -1) {
    nuevaData.labels.push(labels[index]);
nuevaData.datasets[0].data.push(valores[index]);
nuevaData.datasets[0].backgroundColor.push(colores[index]);
graficoVentas.data = nuevaData;
        }
    }

graficoVentas.update();
}
