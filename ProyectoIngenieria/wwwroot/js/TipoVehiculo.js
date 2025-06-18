var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            "url": "/TipoVehiculo/GetAll"
        },
        "columns": [
            { "data": "tipo", "width": "30%" },
            { "data": "descripcion", "width": "30%" },
            
            {
                "data": "id",
                "render": function (data) {
                    return `
                                <a href="/TipoVehiculo/Upsert/${data}" class="btn btn-success btn-sm mx-2" title="Editar">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                          `
                },
                "width": "25%"
            }
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }

    });
}