document.getElementById("descargar-pdf").addEventListener("click", async () => {
	const { jsPDF } = window.jspdf;
	const pdf = new jsPDF("p", "mm", "a4");
	let y = 10;

	// Lista de IDs de los gráficos a capturar
	const graficos = [
		{ id: "column-chart", titulo: "Ventas por semana (Bar)" },
		{ id: "line-chart", titulo: "Ventas por semana por usuario (Línea)" },
		{ id: "pie-chart", titulo: "Ventas por producto (Pie)" },
		{ id: "stack-chart", titulo: "Productos vendidos por usuario (Stacked)" }
	];

	for (let i = 0; i < graficos.length; i++) {
		const { id, titulo } = graficos[i];
		const chartEl = document.getElementById(id);
		if (!chartEl) continue;

		// Título del gráfico
		pdf.setFontSize(14);
		pdf.text(titulo, 10, y);
		y += 6;

		// Capturar gráfico como imagen
		await html2canvas(chartEl, { scale: 2 }).then(canvas => {
			const imgData = canvas.toDataURL("image/png");
			const imgProps = pdf.getImageProperties(imgData);
			const pdfWidth = 180;
			const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;

			if (y + pdfHeight > 290) {
				pdf.addPage();
				y = 10;
			}

			pdf.addImage(imgData, "PNG", 10, y, pdfWidth, pdfHeight);
			y += pdfHeight + 10;
		});
	}

	pdf.save("reportes_ventas.pdf");
});