var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            "url": "/Admin/Operador/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "cedula", "width": "15%" },
            { "data": "nombre", "width": "15%" },
            {
                "data": "cedula",
                "render": function (data) {
                    return `
                            <a href="/Admin/Operador/DocumentoOperador/${data}" class="btn btn-secondary btn-sm mx-2" title="Documentos">
                                <i class="bi bi-folder2-open"></i>
                            </a>

                            <a href="/Admin/Operador/Upsert/${data}" class="btn btn-primary btn-sm mx-2" title="Editar">
                                <i class="bi bi-pencil-square"></i>
                            </a>

                            <a onClick="Delete(${data})" class="btn btn-danger btn-sm mx-2" title="Eliminar">
                                <i class="bi bi-trash"></i>
                            </a>
                          `
                },
                "width": "25%"
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
        text: "El operador será eliminado definitivamente.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, eliminar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Operador/Delete/" + id,
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