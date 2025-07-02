var dataTable;

$(document).ready(function () {
    dataTable = $('#taskTable').DataTable({
        ajax: {
            url: '/RegistroOperadores/GetAll',
            dataSrc: 'data'
        },
        columns: [
            { data: 'fechaInicio', title: 'Fecha Inicio' },
            { data: 'fechaFin', title: 'Fecha Fin' },
            { data: 'nombreVehiculo', title: 'Vehículo' },
            { data: 'placaVehiculo', title: 'Placa'},
            { data: 'nombreOperador', title: 'Operador' },
            {
                data: 'id',
                render: function (data) {
                    return `
                        <a href="/RegistroOperadores/Upsert/${data}" class="btn btn-success btn-sm mx-2" title="Editar">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                        <a onclick="eliminar('/RegistroOperadores/Delete/${data}')" class="btn btn-sm btn-danger">
                            <i class="bi bi-trash"></i> Eliminar
                        </a>`;
                },
                orderable: false,
                searchable: false,
                title: 'Acciones'
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
