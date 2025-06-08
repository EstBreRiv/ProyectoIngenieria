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
                                <a href="/RegistroCombustible/Upsert/${data}?vehiculoId=${vehiculoId}" class="btn btn-sm btn-primary">
                                    <i class="bi bi-pencil-square me-1"></i> Editar
                                </a>
                                <a onClick=Delete("/RegistroCombustible/Delete/${data}") class="btn btn-danger mx-2">
                                    <i class="bi bi-trash"></i> Eliminar
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

function Delete(url) {
    Swal.fire({
        title: '¿Está seguro?',
        text: "¡No podrá recuperar el registro eliminado!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}