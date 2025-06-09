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
            { "data": "modelo", "width": "15%" },
            { "data": "estado", "width": "10%" },
            { "data": "descripcion", "width": "30%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/Vehiculo/DetalleVehiculo/${data}" class="btn btn-success btn-sm mx-2" title="Ver detalles">
                            <i class="bi bi-info-circle"></i>
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
//commit