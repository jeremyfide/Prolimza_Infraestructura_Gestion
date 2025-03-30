document.getElementById('addMateriaCompraBtn').addEventListener('click', function () {
    const container = document.getElementById('materiasCompradasContainer');
    const item = container.querySelector('.materia-item');
    const clone = item.cloneNode(true);
    clone.querySelectorAll('input, select').forEach(el => el.value = '');
    container.appendChild(clone);
});

document.getElementById('addProductoCompraBtn').addEventListener('click', function () {
    const container = document.getElementById('productosCompradosContainer');
    const item = container.querySelector('.producto-item');
    const clone = item.cloneNode(true);
    clone.querySelectorAll('input, select').forEach(el => el.value = '');
    container.appendChild(clone);
});

document.addEventListener('click', function (e) {
    if (e.target.closest('.remove-item')) {
        const bloque = e.target.closest('.materia-item') || e.target.closest('.producto-item');
        const container = bloque.parentNode;
        if (container.querySelectorAll('.materia-item, .producto-item').length > 1) {
            bloque.remove();
        }
    }
});