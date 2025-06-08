var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            "url": "/Vehiculo/GetAll"
        },
        "columns": [
            { "data": "modelo", "width": "15%" },
            { "data": "estado", "width": "10%" },
            { "data": "descripcion", "width": "15%" },
            { "data": "placa", "width": "10%" },
            { "data": "tipo", "width": "10%" },
            { "data": "empresaNombre", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <a href="/Vehiculo/Upsert/${data}" class="btn btn-primary btn-sm mx-2" title="Editar">
                                <i class="bi bi-pencil-square"></i>
                            </a>

                            <a onClick="Delete(${data})" class="btn btn-danger btn-sm mx-2" title="Eliminar">
                                <i class="bi bi-trash"></i>
                            </a>
                          `
                }
            }
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }

    });
}


function Delete(id) {
    Swal.fire({
        title: "¿Estás seguro?",
        text: "El vehículo quedará inactivo definitivamente.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, desactivar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Vehiculo/Delete/" + id,
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