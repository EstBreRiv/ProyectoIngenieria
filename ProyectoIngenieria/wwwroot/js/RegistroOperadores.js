var dataTable;

$(document).ready(function () {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            url: '/Admin/RegistroOperadores/GetAll',
            dataSrc: 'data'
        },
        columns: [
            { data: 'fechaInicio'},
            { data: 'fechaFin' },
            { data: 'marca' },
            { data: 'modelo' },
            { data: 'placa'},
            { data: 'nombreOperador'},
            {
                data: 'id',
                render: function (data) {
                    return `
                        <a href="/Admin/RegistroOperadores/Upsert/${data}" class="btn btn-success btn-sm mx-2" title="Editar">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                        `;
                },
            }
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }
    });
});

function eliminar(url) {
    if (confirm("¿Deseas eliminar esta asignación de operador?")) {
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
