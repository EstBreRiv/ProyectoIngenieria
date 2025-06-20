var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");

    var vehiculoId = $('#VehiculoIdHidden').val();

    loadDataTable();
});

function loadDataTable() {
    var vehiculoId = $('#VehiculoIdHidden').val();

    dataTable = $('#taskTable').DataTable({
        ajax: {
            url: "/RegistroCombustible/GetAll",
            data: function (d) {
                d.id = vehiculoId;
                d.fechaInicio = $('#fechaInicio').val();
                d.fechaFin = $('#fechaFin').val();
            }
        },
        destroy: true,
        columns: [
            { data: "id" },
            { data: "fechaCompra" },
            { data: "litrosComprados" },
            { data: "precioLitro" },
            { data: "totalPagado" },
            {
                data: "id",
                render: function (data) {
                    return `
                        <a href="/RegistroCombustible/Upsert/${data}?vehiculoId=${vehiculoId}" class="btn btn-success btn-sm mx-2" title="Editar">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                        <a onClick=Delete("/RegistroCombustible/Delete/${data}") class="btn btn-danger btn-sm mx-2" title="Eliminar">
                            <i class="bi bi-trash"></i>
                        </a>`;
                }
            }
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }
    });
}

$('#btnFiltrar').on('click', function () {
    var fechaInicio = $('#fechaInicio').val();
    var fechaFin = $('#fechaFin').val();

    if (!fechaInicio || !fechaFin) {
        Swal.fire({
            icon: 'warning',
            title: 'Fechas incompletas',
            text: 'Debes seleccionar tanto la fecha de inicio como la fecha de fin.',
        });
        return;
    }

    dataTable.ajax.reload();
});


$('#btnLimpiar').click(function () {
    $('#fechaInicio').val('');
    $('#fechaFin').val('');
    dataTable.ajax.reload();
});



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