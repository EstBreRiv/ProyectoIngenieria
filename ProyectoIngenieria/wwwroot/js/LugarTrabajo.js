var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            "url": "/LugarTrabajo/GetAll"
        },
        "columns": [

            { "data": "nombre", "width": "40%" },
            { "data": "provincia", "width": "30%" },
            { "data": "canton", "width": "30%" },

            {
                "data": "id",
                "render": function (data) {
                    return `

                        <a href="/Vehiculo/DetalleVehiculo/${data}" class="btn btn-success btn-sm mx-2" title="Ver detalles">
                            <i class="bi bi-info-circle"></i>

                        <a href="/Vehiculo/DetalleVehiculo/${data}" class="btn btn-info btn-sm mx-2" title="Ver detalles">
                            <i class="bi bi-info-circle"></i> Detalles

                        </a>
                    `;
                },
                "width": "20%"
            }
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }
    });
}

