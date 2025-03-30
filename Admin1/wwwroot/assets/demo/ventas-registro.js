document.addEventListener("DOMContentLoaded", function () {
    const addBtn = document.getElementById("addProductoVentaBtn");
    const template = document.getElementById("plantillaProductoVenta");
    const container = document.getElementById("productosVendidosContainer");

    if (addBtn && template && container) {
        addBtn.addEventListener("click", function () {
            const clone = template.firstElementChild.cloneNode(true);
            container.appendChild(clone);
        });
    }

    document.addEventListener('click', function (e) {
        if (e.target.closest('.remove-item')) {
            const row = e.target.closest('.producto-item');
            if (row && row.parentNode.children.length > 1) {
                row.remove();
            }
        }
    });
});