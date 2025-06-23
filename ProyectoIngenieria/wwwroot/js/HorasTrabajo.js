let dataTable;

$(document).ready(function () {
    const vehiculoId = $("#vehiculoId").val();
    cargarTablaHorasTrabajo(vehiculoId);
});

function cargarTablaHorasTrabajo(vehiculoId) {
    dataTable = $('#tablaHorasTrabajo').DataTable({
        ajax: {
            url: `/HorasTrabajo/GetAll?id=${vehiculoId}`
        },
        columns: [
            { data: "fecha"},
            { data: "horometroInicial"},
            { data: "horometroFinal"},
            { data: "precioHora" },
            { data: "totalHoras" },
            { data: "totalGanancia"},
            { data: "lugar" },
            { data: "tipo"},
            { data: "proyecto" },
            {
                data: "id",
                render: function (data, type, row) {
                    return `
                        <a href="/HorasTrabajo/Upsert?id=${data}&vehiculoId=${vehiculoId}" class="btn btn-sm btn-warning me-1">
                            <i class="bi bi-pencil-square"></i> Editar
                        </a>
                        <button onclick="Delete(${data})" class="btn btn-sm btn-danger">
                            <i class="bi bi-trash"></i> Eliminar
                        </button>
                    `;
                },
                orderable: false,
                searchable: false,
                width: "10%"
            }
        ],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }
    });
}

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