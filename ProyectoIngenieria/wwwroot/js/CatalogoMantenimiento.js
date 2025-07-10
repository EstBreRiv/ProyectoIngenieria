var dataTable;

$(document).ready(function () {
    dataTable = $('#tablaCatalogo').DataTable({
        ajax: {
            url: '/Admin/CatalogoMantenimiento/GetAll'
        },
        columns: [
            { data: 'nombre', width: '30%' },
            { data: 'descripcion', width: '50%' },
            {
                data: 'id',
                render: function (data) {
                    return `
                        <a href="/Admin/CatalogoMantenimiento/Upsert/${data}" class="btn btn-success btn-sm mx-2" title="Editar">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                        `;
                },
                width: '20%'
            }
        ],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }
    });
});

function eliminar(url) {
    if (confirm('¿Deseas eliminar este mantenimiento del catálogo?')) {
        $.ajax({
            url: url,
            type: 'DELETE',
            success: function (data) {
                if (data.success) {
                    dataTable.ajax.reload();
                } else {
                    alert(data.message);
                }
            }
        });
    }
}
