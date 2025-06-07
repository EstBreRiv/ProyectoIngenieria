var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");

    // Obtener el ID del vehículo desde el campo oculto
    var vehiculoId = $('#VehiculoIdHidden').val();

    loadDataTable();
});

function loadDataTable() {
    var vehiculoId = $('#VehiculoIdHidden').val(); // Obtener el ID del vehículo

    dataTable = $('#taskTable').DataTable({
        ajax: {
            url: "/RegistroCombustible/GetAll",
            data: function (d) {
                d.id = vehiculoId; // Pasar el ID del vehículo al backend
            }
        },

        "columns": [
            { "data": "id", "width": "15%" },
            { "data": "fechaCompra", "width": "15%" },
            { "data": "litrosComprados", "width": "15%" },
            { "data": "precioLitro", "width": "15%" },
            { "data": "totalPagado", "width": "15%" },

            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/RegistoCombustible/Upsert/${data}" class="bg-blue-500 text-white px-3 py-1 rounded hover:bg-blue-600 mx-1 text-xs">
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