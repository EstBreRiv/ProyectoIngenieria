var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            "url": "/Vehiculo/GetAll"
        },
        "columns": [
            { "data": "id", "width": "15%" },
            { "data": "modelo", "width": "15%" },
            { "data": "estado", "width": "15%" },
            { "data": "descripcion", "width": "15%" },
            { "data": "placa", "width": "15%" },
            { "data": "tipo", "width": "15%" },
            { "data": "empresaId", "width": "15%" },
            {
                "render": function (data) {
                    return `
                            <a href="/Admin/Vehicle/upsert/${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>

                            <a onClick=Delete(${data}) class="btn btn-danger mx-2">
                                <i class="bi bi-trash"></i> Delete
                            </a>
                          `
                }
            }
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }

    });
}