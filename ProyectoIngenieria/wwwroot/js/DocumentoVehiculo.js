var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    const vehiculoId = $('#vehiculoId').val();

    dataTable = $('#taskTable').DataTable({
        ajax: {
            url: `/Admin/Vehiculo/GetDocumentosVehiculo?id=${vehiculoId}`,
            type: "GET",
            datatype: "json"
        },
        "columns": [
            { "data": "nombre", "width": "50%" },
            {
                "data": "ruta",
                "render": function (ruta, type, row) {
                    return `
                <a href="${ruta}" class="btn btn-success btn-sm mx-2" title="Ver" target="_blank">
                    <i class="bi bi-eye"></i>
                </a>
                <a onClick="Delete(${row.id})" class="btn btn-danger btn-sm mx-2" title="Eliminar">
                    <i class="bi bi-trash"></i>
                </a>
            `
                },
                "width": "20%"
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
        text: "Este documento será eliminado definitivamente.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, eliminar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Admin/Vehiculo/DeleteDocumento/" + id,
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