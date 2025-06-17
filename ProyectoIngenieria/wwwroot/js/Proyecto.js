var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            "url": "/Admin/Proyecto/GetAll"
        },
        "columns": [

            { "data": "nombreProyecto", "width": "40%" },
            { "data": "cliente", "width": "30%" },
            { "data": "fechaInicio", "width": "30%" },

            {
                "data": "id",
                "render": function (data) {
                    return `

                        <a href="/Admin/Vehiculo/DetalleVehiculo/${data}" class="btn btn-success btn-sm mx-2" title="Ver detalles">
                            <i class="bi bi-info-circle"></i>

                        <a href="/Admin/Vehiculo/DetalleVehiculo/${data}" class="btn btn-info btn-sm mx-2" title="Ver detalles">
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

