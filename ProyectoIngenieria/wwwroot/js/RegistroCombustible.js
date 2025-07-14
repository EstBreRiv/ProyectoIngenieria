let dataTable;

$(document).ready(function () {
    const vehiculoId = $("#VehiculoIdHidden").val();

    inicializarTabla(vehiculoId);

    $("#btnFiltrar").on("click", function () {
        const fechaInicio = $("#filtroInicio").val();
        const fechaFin = $("#filtroFin").val();

        if (!fechaInicio || !fechaFin) {
            Swal.fire({
                icon: 'warning',
                title: 'Fechas incompletas',
                text: 'Debes seleccionar tanto la fecha de inicio como la fecha de fin.',
            });
            return;
        }

        dataTable.ajax.reload();
    });

    $("#btnLimpiar").on("click", function () {
        $("#filtroInicio").val('');
        $("#filtroFin").val('');
        dataTable.ajax.reload();
    });
});

function inicializarTabla(vehiculoId) {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            url: "/Admin/RegistroCombustible/GetAll",
            data: function (d) {
                d.id = vehiculoId;
                d.fechaInicio = $("#filtroInicio").val(); // formato: YYYY-MM-DD
                d.fechaFin = $("#filtroFin").val();
            },
            dataSrc: "data"
        },
        destroy: true,
        columns: [
            { data: "fechaCompra", width: "15%" },
            { data: "marca", width: "15%" },
            { data: "modelo", width: "15%" },
            { data: "placa", width: "15%" },
            {
                data: "id",
                render: function (data) {
                    return `
                        <a href="/Admin/RegistroCombustible/Upsert/${data}?vehiculoId=${$("#VehiculoIdHidden").val()}" class="btn btn-success btn-sm mx-2" title="Editar">
                            <i class="bi bi-pencil-square"></i>
                        </a>

                        <a href="/Admin/RegistroCombustible/DetalleRegistroCombustible/${data}" class="btn btn-success btn-sm mx-2" title="Ver detalles">
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