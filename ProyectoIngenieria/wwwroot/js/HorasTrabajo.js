let dataTable;

$(document).ready(function () {
    const vehiculoId = $("#vehiculoId").val();

    inicializarTabla(vehiculoId);

    $("#btnFiltrar").on("click", function () {
        dataTable.ajax.reload();
    });

    $("#btnLimpiar").on("click", function () {
        $("#filtroInicio").val('');
        $("#filtroFin").val('');
        dataTable.ajax.reload();
    });
});

function inicializarTabla(vehiculoId) {
    dataTable = $('#tablaHorasTrabajo').DataTable({
        ajax: {
            url: "/Admin/HorasTrabajo/GetAll",
            data: function (d) {
                d.id = vehiculoId;
                d.fechaInicio = $("#filtroInicio").val();
                d.fechaFin = $("#filtroFin").val();
            },
            dataSrc: "data"
        },
        columns: [
            { data: "fecha", width: "10%" },
            { data: "marca", width: "12%" },
            { data: "modelo", width: "12%" },
            { data: "placa", width: "12%" },
            { data: "proyecto", width: "12%" },
            { data: "tipo", width: "12%" },
            { data: "lugar", width: "12%" },
            {
                data: "id",
                render: function (data, type, row) {
                    return `
                        <a href="/Admin/HorasTrabajo/Upsert?id=${data}&vehiculoId=${row.id}" class="btn btn-success btn-sm mx-2" title="Editar">
                            <i class="bi bi-pencil-square"></i>
                        </a>

                        <a href="/Admin/HorasTrabajo/DetalleHorasTrabajo/${data}" class="btn btn-success btn-sm mx-2" title="Ver detalles">
                            <i class="bi bi-info-circle"></i>
                        </a>
                   `;
                },
                "width": "07%"
            }
        ],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }
    });
}
