var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            "url": "/Admin/Vehiculo/GetAll"
        },
        "columns": [

            { "data": "marca", "width": "15%" },
            { "data": "modelo", "width": "15%" },
            { "data": "placa", "width": "15%" },
            { "data": "estado", "width": "10%" },
            { "data": "tipoVehiculo", "width": "15%" },
            { "data": "empresa", "width": "25%" },

            {
                "data": "id",
                "render": function (data) {
                    return `

                        <a href="/Admin/Vehiculo/DetalleVehiculo/${data}" class="btn btn-success btn-sm mx-2" title="Ver detalles">
                            <i class="bi bi-info-circle"></i>
                        </a>
                    `;
                },
                "width": "20%"
            }
        ],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }

    });
}

$('#empresaFilter').on('change', function () {
    const empresa = $(this).val();

    if (empresa) {
        dataTable.column(5).search(empresa).draw();
    } else {
        dataTable.column(5).search('').draw();
    }
});
