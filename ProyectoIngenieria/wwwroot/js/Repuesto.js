var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tablaRepuesto').DataTable({
        "ajax": {
            "url": "/Repuesto/GetAll"
        },
        "columns": [
            { "data": "nombre", "width": "25%" },
            { "data": "descripcion", "width": "35%" },
            {
                "data": "precioEstimado",
                "render": function (data) {
                    return '₡' + parseFloat(data).toFixed(2);
                },
                "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Repuesto/Upsert/${data}" class="text-blue-600 hover:underline mx-1">Editar</a>
                            <a onclick=Eliminar("/Repuesto/Delete/${data}") class="text-red-600 hover:underline mx-1 cursor-pointer">Eliminar</a>
                        </div>`;
                },
                "width": "25%"
            }
        ],
        "language": {
            "url": 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }
    });
}

function Eliminar(url) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "Este repuesto será eliminado permanentemente",
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
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
