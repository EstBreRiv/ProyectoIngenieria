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
            url: "/HorasTrabajo/GetAll",
            data: function (d) {
                d.id = vehiculoId;
                d.fechaInicio = $("#filtroInicio").val();
                d.fechaFin = $("#filtroFin").val();
            },
            dataSrc: "data"
        },
        columns: [
            { data: "fecha", width: "12%" },
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
                        <a href="/HorasTrabajo/Upsert?id=${data}&vehiculoId=${row.id}" class="btn btn-success btn-sm mx-2" title="Editar">
                            <i class="bi bi-pencil-square"></i>
                        </a>

                        <a href="/HorasTrabajo/DetalleHorasTrabajo/${data}" class="btn btn-success btn-sm mx-2" title="Ver detalles">
                            <i class="bi bi-info-circle"></i>
                        </a>
                    `;
                },
             
            }
        ],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }
    });
}

/*
<button onclick="Delete(${data})" class="btn btn-sm btn-danger" title="Eliminar">
    <i class="bi bi-trash"></i> Eliminar
</button>
*/

function Delete(id) {
    Swal.fire({
        title: "¿Estás seguro?",
        text: "Este registro será eliminado definitivamente.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, eliminar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/HorasTrabajo/Delete/${id}`,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function () {
                    toastr.error("Error al procesar la solicitud");
                }
            });
        }
    });
}
