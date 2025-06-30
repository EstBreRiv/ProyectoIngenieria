var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            "url": "/Proyecto/GetAll"
        },
        "columns": [

            { "data": "nombreProyecto", "width": "40%" },
            { "data": "cliente", "width": "30%" },
            { "data": "fechaInicio", "width": "30%" },

            {
                "data": "id",
                "render": function (data, type, row) {
                    return `
                        <a href="/Proyecto/Upsert?id=${data}&vehiculoId=${row.id}" class="btn btn-sm btn-warning me-1">
                            <i class="bi bi-pencil-square"></i> Editar
                        </a>
                        <button onclick="Delete(${data})" class="btn btn-sm btn-danger">
                            <i class="bi bi-trash"></i> Eliminar
                        </button>
                    `;
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
        text: "Este proyecto será eliminado definitivamente.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, eliminar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Proyecto/Delete/${id}`,
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
