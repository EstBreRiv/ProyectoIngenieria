var dataTable;

$(document).ready(function () {
    console.log("Task.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            "url": "/Admin/Proyecto/GetAll"
        },
        "columns": [
            { "data": "nombreProyecto", "width": "20%" },
            { "data": "cliente", "width": "20%" },
            { "data": "fechaInicio", "width": "15%" },
            { "data": "fechaFin", "width": "15%" },

            {
                "data": null,
                "render": function (data, type, row) {
                    const hoy = new Date();
                    const fechaInicio = new Date(row.fechaInicio);
                    const fechaFin = new Date(row.fechaFin);
                    let estado = "";

                    if (hoy < fechaInicio) {
                        estado = "No iniciado";
                    } else if (hoy > fechaFin) {
                        estado = "Finalizado";
                    } else {
                        estado = "En proceso";
                    }

                    row.estadoProyecto = estado;

                    return `<span class="badge ${estado === 'Finalizado' ? 'bg-danger' :
                        estado === 'En proceso' ? 'bg-success' : 'bg-warning'
                        }">${estado}</span>`;
                },
                "width": "15%"
            },

            {
                "data": "id",
                "render": function (data, type, row) {
                    return `
                        <a href="/Admin/Proyecto/Upsert?id=${data}" class="btn btn-success btn-sm mx-2" title="Editar">
                            <i class="bi bi-pencil-square"></i>
                        </a>

                        <button onclick="Eliminar('/Admin/Proyecto/Eliminar/${data}')" class="btn btn-danger btn-sm mx-2" title="Eliminar">
                            <i class="bi bi-trash"></i>
                        </button>
                    `;
                },
                "width": "15%"
            }
        ],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        },
        rowCallback: function (row, data) {
            const hoy = new Date();
            const fechaInicio = new Date(data.fechaInicio);
            const fechaFin = new Date(data.fechaFin);

            if (hoy < fechaInicio) {
                data.estadoProyecto = "No iniciado";
            } else if (hoy > fechaFin) {
                data.estadoProyecto = "Finalizado";
            } else {
                data.estadoProyecto = "En proceso";
            }
        }
    });

    $('#estadoFiltro').on('change', function () {
        const selected = $(this).val();
        dataTable.column(4).search(selected).draw();
    });
}

function Eliminar(url) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "Este proyecto será eliminado permanentemente",
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
