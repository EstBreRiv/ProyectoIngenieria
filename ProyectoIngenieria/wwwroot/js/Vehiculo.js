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
            { "data": "id", "width": "15%" },
            { "data": "modelo", "width": "15%" },
            { "data": "estado", "width": "15%" },
            { "data": "descripcion", "width": "15%" },
            { "data": "placa", "width": "15%" },
            { "data": "tipo", "width": "15%" },
            { "data": "empresaNombre", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <a href="/Vehiculo/Upsert/${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Editar
                            </a>

                            <a href="/RegistroCombustible/Index" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Registro de combustible
                            </a>

                            <a onClick=Delete(${data}) class="btn btn-danger mx-2">
                                <i class="bi bi-trash"></i> Eliminar
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
                url: "/Controllers/VehiculoController/Delete/" + id,
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