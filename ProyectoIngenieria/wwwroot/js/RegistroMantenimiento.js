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
            url: "/RegistroMantenimiento/GetAll",
            data: function (d) {
                d.id = vehiculoId; // ID del vehículo
                d.fechaInicio = $('#filtroInicio').val();
                d.fechaFin = $('#filtroFin').val();
            }
        },
        columns: [
            { data: "vehiculoModelo", width: "15%", title: "Vehículo" },
            { data: "descripcion", width: "25%", title: "Descripción" },
            { data: "fecha", width: "25%", title: "Fecha" },
            { data: "precio", width: "20%", title: "Precio" },
            {
                data: "id",
                render: function (data) {
                    return `
                        <a href="/RegistroMantenimiento/Details/${data}" class="btn btn-success btn-sm mx-2" title="Ver detalles">
                             <i class="bi bi-info-circle"></i>
                        </a>
                    `;
                },
                width: "10%"
            }
        ],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }
    });
}
