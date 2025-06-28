var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            "url": "/Admin/Operador/GetRegistroOperadores",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "operadorCedula", "width": "15%" },
            { "data": "operadorNombre", "width": "15%" },
            { "data": "vehiculoModelo", "width": "15%" },
            { "data": "fecha", "width": "15%" },
            {
                "data": "vehiculoId",
                "render": function (data) {
                    return `
                            <a href="/Admin/Vehiculo/DetalleVehiculo/${data}" class="btn btn-success btn-sm mx-2" title="Ver Vehiculo">
                                <i class="bi bi-eye"></i>
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