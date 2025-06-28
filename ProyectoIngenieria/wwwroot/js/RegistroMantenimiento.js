var dataTable;

$(document).ready(function () {
    console.log("RegistroMantenimiento.js cargado y listo");
    loadDataTable();

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

function loadDataTable() {
    dataTable = $('#tablaMantenimiento').DataTable({
        ajax: {
            url: "/RegistroMantenimiento/GetAll",
            data: function (d) {
                d.fechaInicio = $('#filtroInicio').val();
                d.fechaFin = $('#filtroFin').val();
            }
        },
        columns: [
            { data: "vehiculoModelo", width: "15%", title: "Vehículo" },
            { data: "descripcion", width: "25%", title: "Descripción" },
            { data: "fecha", width: "25%", title: "Fecha" },
            { data: "precio", width: "10%", title: "Precio" },
            {
                data: "id",
                render: function (data) {
                    return `
                        <a href="/RegistroMantenimiento/Details/${data}" class="btn btn-sm bg-blue-600 text-white rounded px-3 py-1 hover:bg-blue-700" title="Editar">
                            <i class="bi bi-pencil-square"></i> Detalles
                        </a>
                    `;
                },
                width: "15%", title: "Acciones"
            }
        ],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }
    });
}
