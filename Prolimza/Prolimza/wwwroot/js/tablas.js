$(document).ready(function () {
    $('#miTabla').DataTable({
        dom: 'Bfrtip', // ← importante: define la ubicación de los botones
        buttons: [
            {
                extend: 'copyHtml5',
                text: 'Copiar',
                exportOptions: { columns: ':visible' }
            },
            {
                extend: 'excelHtml5',
                text: 'Exportar a Excel',
                exportOptions: { columns: ':visible' }
            },
            {
                extend: 'pdfHtml5',
                text: 'Exportar a PDF',
                exportOptions: { columns: ':visible' }
            },
            {
                extend: 'print',
                text: 'Imprimir',
                exportOptions: { columns: ':visible' }
            }
        ],
        language: {
            decimal: ",",
            thousands: ".",
            search: "Buscar:",
            lengthMenu: "Mostrar _MENU_ registros",
            info: "Mostrando _START_ a _END_ de _TOTAL_ registros",
            infoEmpty: "Mostrando 0 a 0 de 0 registros",
            infoFiltered: "(filtrado de _MAX_ registros totales)",
            loadingRecords: "Cargando...",
            processing: "Procesando...",
            zeroRecords: "No se encontraron resultados",
            emptyTable: "No hay datos disponibles en la tabla",
            paginate: {
                first: "Primero",
                previous: "Anterior",
                next: "Siguiente",
                last: "Último"
            },
            aria: {
                sortAscending: ": activar para ordenar ascendente",
                sortDescending: ": activar para ordenar descendente"
            },
            buttons: {
                copy: "Copiar",
                excel: "Excel",
                pdf: "PDF",
                print: "Imprimir"
            }
        },
        paging: true,
        searching: true,
        ordering: true,
        info: true,
        lengthChange: false,
        pageLength: 15
    });
});
