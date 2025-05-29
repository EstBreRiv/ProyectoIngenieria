var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            "url": "/Empresa/GetAll"
        },
        "columns": [
            { "data": "id", "width": "15%" },
            { "data": "nombre", "width": "30%" },
            
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Empresa/Upsert/${data}" class="bg-blue-500 text-white px-3 py-1 rounded hover:bg-blue-600 mx-1 text-xs">
                                    <i class="bi bi-pencil-square"></i> Editar
                                </a>
                            </div>

                          `
                }
            }
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }

    });
}