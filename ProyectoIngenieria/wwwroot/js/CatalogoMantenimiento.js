var dataTable;

$(document).ready(function () {
    dataTable = $('#tablaCatalogo').DataTable({
        ajax: {
            url: '/CatalogoMantenimiento/GetAll'
        },
        columns: [
            { data: 'nombre', width: '30%' },
            { data: 'descripcion', width: '50%' },
            {
                data: 'id',
                render: function (data) {
                    return `
                        <a href="/CatalogoMantenimiento/Upsert/${data}" class="text-blue-600 hover:underline mr-2">Editar</a>
                        <a onclick="eliminar('/CatalogoMantenimiento/Delete/${data}')" class="text-red-600 hover:underline">Eliminar</a>
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
