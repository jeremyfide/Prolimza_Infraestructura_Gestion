
function renderizarGraficoPrediccion() {
	const chartElement = document.querySelector("#prediccion-line-chart");
	if (!chartElement) return;

	const rawLabels = chartElement.dataset.labels;
	const rawValores = chartElement.dataset.valores;

	const labels = rawLabels ? rawLabels.split(",") : [];
	const valores = rawValores ? rawValores.split(",").map(Number) : [];

	console.log("📊 Labels:", labels);
	console.log("📊 Valores:", valores);

	if (valores.some(isNaN) || valores.length === 0) {
		console.warn("⚠️ Valores inválidos o vacíos. No se renderiza el gráfico.");
		return;
	}

	// Destruye gráfico anterior si existe
	if (window.prediccionChartInstance) {
		window.prediccionChartInstance.destroy();
	}

	window.prediccionChartInstance = new ApexCharts(chartElement, {
		series: [{
			name: "Unidades predichas",
			data: valores
		}],
		chart: {
			height: 300,
			type: 'line',
			toolbar: {
				show: true,
				tools: {
					download: true,  // botón de descarga
					selection: false,
					zoom: false,
					zoomin: false,
					zoomout: false,
					pan: false,
					reset: false,
				}
			},
			animations: {
				enabled: true,
				easing: 'easeinout',
				speed: 800
			}
		},
		xaxis: {
			categories: labels,
			labels: {
				rotate: -45,
				style: {
					fontSize: '11px'
				}
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
			y: {
				formatter: val => `${val.toFixed(2)} unidades`
			}
		},
		legend: {
			position: 'top'
		}
	});

	// Renderiza el gráfico
	window.prediccionChartInstance.render().catch(err => {
		console.error("🛑 Error al renderizar gráfico:", err);
	});
}