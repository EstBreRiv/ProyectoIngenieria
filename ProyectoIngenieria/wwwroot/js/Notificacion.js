var dataTable;

$(document).ready(function () {
    dataTable = $('#notificacionesTable').DataTable({
        ajax: {
            url: "/Admin/Notificacion/GetAll",
            dataSrc: "data"
        },
        columns: [
            {
                data: "id",
                orderable: false,
                render: function (data) {
                    return `<input type="checkbox" class="noti-checkbox" value="${data}" />`;
                }
            },
            { data: "titulo" },
            { data: "descripcion" },
            { data: "fecha", "width": "8%" },
            { data: "placa", "width": "8%" },
            { data: "modelo" }
        ],
        rowCallback: function (row, data) {
            if (!data.leida) {
                $(row).addClass('table-secondary');
                $(row).find('td:eq(1)').html(`<strong><i class="bi bi-dot text-danger me-1"></i> ${data.titulo}</strong>`);
            }
        },
        order: [[3, "desc"]],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }
    });
});

/*Logica para eliminar por checkbox*/
$('#checkAll').on('click', function () {
    $('.noti-checkbox').prop('checked', this.checked);
});

$('#btnEliminarSeleccionadas').on('click', function () {
    const ids = $('.noti-checkbox:checked').map(function () {
        return this.value;
    }).get();

    if (ids.length === 0) {
        Swal.fire({
            icon: 'warning',
            title: 'Sin selección',
            text: 'Debes seleccionar al menos una notificación para eliminar.',
        });
        return;
    }

    Swal.fire({
        title: '¿Estás seguro?',
        text: "Las notificaciones seleccionadas serán eliminadas permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch("/Admin/Notificacion/EliminarSeleccionadas", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(ids)
            })
                .then(res => {
                    if (res.ok) {
                        toastr.success("Notificaciones eliminadas correctamente.");
                        dataTable.ajax.reload();
                    } else {
                        toastr.error("Error al eliminar notificaciones.");
                    }
                });
        }
    });
});