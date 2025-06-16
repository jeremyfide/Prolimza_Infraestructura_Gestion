function cerrarDetalle() {
    var modal = bootstrap.Modal.getInstance(document.getElementById('detalleModal'));
    if (modal) {
        modal.hide();
    }
}

$(document).ready(function () {
    $("a[id^='btn-detalle-']").click(function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var controller = $(this).data('controller');

        $.ajax({
            url: '/' + controller + '/Details/' + id,
            type: 'GET',
            success: function (result) {
                $('#detalle-container').html(result);
                var myModal = new bootstrap.Modal(document.getElementById('detalleModal'));
                myModal.show();
            },
            error: function () {
                alert("Error al cargar los detalles.");
            }
        });
    });
});
