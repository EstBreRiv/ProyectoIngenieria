var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            "url": "/DocumentoOperador/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "nombre", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <a href="/Operador/Documentos/${data}" class="btn btn-success btn-sm mx-2" title="Ver">
                        <i class="bi bi-eye"></i>
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
        title: "Esta seguro de querer eliminar?",
        text: "Los datos no seran eliminados de la base de datos, pero seran invisibles para el usuario",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, estoy seguro!"
    }).then((result) => {
        if (result.isConfirmed) {

            //metodo que permite hacer el delete sin tener que hacer un httpget
            $.ajax({
                url: "/Operador/Delete/" + id,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                },
                error: function (data) {
                    toastr.errort(data.message);
                }
            });


        }
    });

}