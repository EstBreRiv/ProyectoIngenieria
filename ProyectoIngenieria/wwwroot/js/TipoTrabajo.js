var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            "url": "/Admin/TipoTrabajo/GetAll"
        },
        "columns": [
            { "data": "nombre", "width": "30%" },
            { "data": "descripcion", "width": "50%" },

            {
                "data": "id",
                "render": function (data) {
                    return `
                                <a href="/Admin/TipoTrabajo/Upsert/${data}" class="btn btn-success btn-sm mx-2" title="Editar">
                                    <i class="bi bi-pencil-square"></i>
                                </a>

                                <button onclick="Eliminar('/Admin/TipoTrabajo/Eliminar/${data}')" class="btn btn-danger btn-sm mx-2" title="Eliminar">
                                <i class="bi bi-trash"></i>
                                </button>
                          `
                },
                "width": "25%"
            }
        ],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }

    });
}


function Eliminar(url) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "Este tipo de trabajo será eliminado permanentemente",
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
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message || "Ocurrio un error al eliminar");
                    }
                }
            });
        }
    });
}
