var dataTable;

$(document).ready(function () {
    dataTable = $('#notificacionesTable').DataTable({
        ajax: {
            url: "/Notificacion/GetAll",
            dataSrc: "data"
        },
        columns: [
            { data: "titulo" },
            { data: "descripcion" },
            { data: "fecha" },
            { data: "placa" },
            { data: "modelo" }
        ],
        rowCallback: function (row, data) {
            if (!data.leida) {
                $(row).addClass('table-secondary');
                $(row).find('td:first').html(`<strong><i class="bi bi-dot text-danger me-1"></i> ${data.titulo}</strong>`);
            }
        },
        order: [[2, "desc"]],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }
    });
});