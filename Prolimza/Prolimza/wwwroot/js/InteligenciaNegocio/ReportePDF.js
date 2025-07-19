document.getElementById("descargar-pdf").addEventListener("click", async () => {
	try
	{  
		const { jsPDF } = window.jspdf;
		const pdf = new jsPDF("p", "mm", "a4");
		let y = 10;

		const graficos = ["column-chart", "line-chart", "pie-chart", "stack-chart"];

		const pageTitle = document.querySelector(".geex-content__header__title")?.textContent.trim() || "Reporte";
		const fecha = new Date();
		const yyyy = fecha.getFullYear();
		const mm = String(fecha.getMonth() + 1).padStart(2, "0");
		const dd = String(fecha.getDate()).padStart(2, "0");
		const hh = String(fecha.getHours()).padStart(2, "0");
		const min = String(fecha.getMinutes()).padStart(2, "0");
		const ss = String(fecha.getSeconds()).padStart(2, "0");
		const fechaTexto = `${yyyy}-${mm}-${dd}`;
		const fechaHora = `${yyyy}-${mm}-${dd}_${hh}-${min}-${ss}`;

		pdf.setFontSize(22);
		pdf.text(pageTitle, 20, 60);
		pdf.setFontSize(14);
		pdf.text(`Fecha de generación: ${fechaTexto}`, 20, 70);
		pdf.addPage();

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

			y += 10;
			pdf.setFontSize(14);
			pdf.text(titulo, 10, y);
			y += 5;

			await html2canvas(chartEl, { scale: 2 }).then(canvas => {
				const imgData = canvas.toDataURL("image/png");
				const pdfWidth = 160;
				const pdfHeight = 90;

				pdf.addImage(imgData, "PNG", 10, y, pdfWidth, pdfHeight);
				y += pdfHeight + 6;

				if ((i + 1) % 2 === 0 && i !== graficos.length - 1) {
					pdf.addPage();
					y = 10;
				}
			});
		}

		pdf.save(`${pageTitle.replace(/\s+/g, "_")}_${fechaHora}.pdf`);

		Swal.fire({
			title: 'PDF generado',
			text: 'El archivo se descargó correctamente.',
			icon: 'success',
			toast: true,
			position: 'bottom-end',
			showConfirmButton: false,
			timer: 3000,
			timerProgressBar: true
		});
	} catch (error) {
		console.error("❌ Error generando PDF:", error);
		Swal.fire({
			title: 'Error',
			text: 'Hubo un problema generando el PDF.',
			icon: 'error',
			toast: true,
			position: 'bottom-end',
			showConfirmButton: false,
			timer: 4000,
			timerProgressBar: true
		});
	}
});
