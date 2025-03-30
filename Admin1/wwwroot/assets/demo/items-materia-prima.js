document.getElementById('addMateriaPrimaBtn').addEventListener('click', function () {
    const container = document.getElementById('materiasPrimasContainer');
const item = container.querySelector('.materia-prima-item');
const clone = item.cloneNode(true);

    // Limpiar valores del clone
    clone.querySelectorAll('input, select').forEach(el => el.value = '');
container.appendChild(clone);
});

document.addEventListener('click', function (e) {
    if (e.target.closest('.remove-item')) {
        const items = document.querySelectorAll('.materia-prima-item');
        if (items.length > 1) {
    e.target.closest('.materia-prima-item').remove();
        }
    }
});
