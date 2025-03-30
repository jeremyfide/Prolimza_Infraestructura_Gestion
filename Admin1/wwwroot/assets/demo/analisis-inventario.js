const ctxInv = document.getElementById("graficoInventarioCantidad").getContext("2d");
new Chart(ctxInv, {
    type: 'bar',
    data: {
        labels: ["Limpiador", "Desinfectante", "Jabón", "Fragancia", "Ácido cítrico"],
        datasets: [{
            label: "Unidades en inventario",
            backgroundColor: "rgba(2,117,216,0.8)",
            borderColor: "rgba(2,117,216,1)",
            data: [320, 280, 150, 90, 60]
        }]
    },
    options: {
        scales: {
            xAxes: [{ gridLines: { display: false } }],
            yAxes: [{ ticks: { beginAtZero: true }, gridLines: { display: true } }]
        },
        legend: { display: false }
    }
});

const ctxPie = document.getElementById("graficoDistribucionInventario").getContext("2d");
new Chart(ctxPie, {
    type: 'pie',
    data: {
        labels: ["Productos terminados", "Materias primas"],
        datasets: [{
            data: [3250, 1140],
            backgroundColor: ["#0d6efd", "#198754"]
        }]
    },
    options: {
        responsive: true,
        legend: { position: 'bottom' }
    }
});