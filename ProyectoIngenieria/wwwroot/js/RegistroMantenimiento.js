var dataTable;

$(document).ready(function () {
    const vehiculoId = $("#VehiculoIdHidden").val();
    console.log("RegistroMantenimiento.js cargado y listo");
    loadDataTable(vehiculoId);

    // Botón de filtro
    $('#btnFiltrar').on('click', function () {
        dataTable.ajax.reload();
    });

    // Botón de limpiar filtros
    $('#btnLimpiar').on('click', function () {
        $('#filtroInicio').val('');
        $('#filtroFin').val('');
        dataTable.ajax.reload();
    });
});

function loadDataTable(vehiculoId) {
    dataTable = $('#tablaMantenimiento').DataTable({
        ajax: {
            url: "/Admin/RegistroMantenimiento/GetAll",
            data: function (d) {
                d.id = vehiculoId; // ID del vehículo
                d.fechaInicio = $('#filtroInicio').val();
                d.fechaFin = $('#filtroFin').val();
            }
        },
        columns: [
            { data: "fecha", width: "15%"  },
            { data: "marca", width: "15%" },
            { data: "modelo", width: "15%" },
            { data: "placa", width: "15%" },
            { data: "descripcion", width: "25%" },
            {
                data: "id",
                render: function (data) {
                    return `

                        <a href="/Admin/RegistroMantenimiento/Upsert/${data}?vehiculoId=${$("#VehiculoIdHidden").val()}" class="btn btn-success btn-sm mx-2" title="Editar">
                            <i class="bi bi-pencil-square"></i>
                        </a>

                        <a href="/Admin/RegistroMantenimiento/Details/${data}" class="btn btn-success btn-sm mx-2" title="Ver detalles">
                             <i class="bi bi-info-circle"></i>
                        </a>
                    `;
                },
                width: "15%"
            }
        ],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }
    });
}
