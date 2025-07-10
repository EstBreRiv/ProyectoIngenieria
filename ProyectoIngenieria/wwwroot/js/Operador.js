$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();

    $('#filtroTipo').on('change', function () {
        dataTable.ajax.reload();
    });
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            url: "/Admin/Operador/GetAll",
            type: "GET",
            datatype: "json",
            data: function (d) {
                d.tipo = $('#filtroTipo').val();
            }
        },
        "columns": [
            { "data": "cedula", "width": "15%" },
            { "data": "nombre", "width": "25%" },
            { "data": "telefono", "width": "15%" },
            { "data": "tipoColaborador", "width": "15%" },
            {
                "data": "cedula",
                "render": function (data, type, row) {
                    const esInactivo = row.tipoColaborador === "Inactivo";
                    const textoBoton = esInactivo ? "Activar" : "Desactivar";
                    const iconoBoton = esInactivo ? "bi bi-check-circle" : "bi bi-x-circle";
                    const funcion = esInactivo ? `Activar('${data}')` : `Delete('${data}')`;

                    return `
                        <a href="/Admin/Operador/DocumentoOperador/${data}" class="btn btn-secondary btn-sm mx-2" title="Documentos">
                            <i class="bi bi-folder2-open"></i>
                        </a>

                        <a href="/Admin/Operador/Upsert/${data}" class="btn btn-success btn-sm mx-2" title="Editar">
                            <i class="bi bi-pencil-square"></i>
                        </a>

                        <a onClick="${funcion}" class="btn btn-danger btn-sm mx-2" title="${textoBoton}">
                            <i class="${iconoBoton}"></i>
                        </a>
                    `;
                }
,
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
        text: "El operador quedará inactivo.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, desactivar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Admin/Operador/Delete/" + id,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                        setTimeout(() => {
                            window.location.href = '/Admin/Operador/Index';
                        }, 20);
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

function Activar(id) {
    Swal.fire({
        title: '¿Está seguro?',
        text: "El operador será activado.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#28a745',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, activar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.post("/Admin/Operador/Activar", { id: id })
                .done(function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => {
                            window.location.href = '/Admin/Operador/Index';
                        }, 20);
                    } else {
                        toastr.error(data.message);
                    }
                })
                .fail(function () {
                    toastr.error("Error al intentar activar.");
                });
        }
    });
}