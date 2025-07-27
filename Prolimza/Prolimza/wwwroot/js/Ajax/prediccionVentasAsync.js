$(document).ready(function () {
    $('#form-prediccion').submit(function (e) {
        e.preventDefault();
        $('#resultado-prediccion').empty();
        $('#barra-carga').show();

        var idProducto = $('#idProducto').val();

        $.ajax({
            url: '/Prediccion/PredecirAsync',
            type: 'POST',
            data: { idProducto },
            success: function (data) {
                $('#barra-carga').hide();
                $('#resultado-prediccion').html(data);

                setTimeout(() => {
                    renderizarGraficoPrediccion();
                }, 50);
            },
            error: function () {
                $('#barra-carga').hide();
                $('#resultado-prediccion').html("<div class='alert alert-danger'>Error al obtener predicción.</div>");
            }
        });


    });
});