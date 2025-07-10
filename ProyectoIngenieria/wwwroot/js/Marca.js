var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            "url": "/Admin/Marca/GetAll"
        },
        "columns": [
            { "data": "nombreMarca", "width": "30%" },
            
            {
                "data": "id",
                "render": function (data) {
                    return `
                                <a href="/Admin/Marca/Upsert/${data}" class="btn btn-success btn-sm mx-2" title="Editar">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                          `
                },
                "width": "25%"
            }
        ],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }

    });
}