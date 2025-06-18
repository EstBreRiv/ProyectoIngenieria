var dataTable;

$(document).ready(function () {
    console.log("RegistroMantenimiento.js cargado y listo");
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tablaMantenimiento').DataTable({
        ajax: {
            "url": "/RegistroMantenimiento/GetAll"
        },
        "columns": [
            { "data": "vehiculoModelo", "width": "15%", title: "Vehículo" },
            { "data": "catalogoMantenimiento", "width": "15%", title: "Tipo de Mantenimiento" },
            { "data": "descripcion", "width": "25%", title: "Descripción" },
            { "data": "fecha", "width": "25%", title: "Fecha" },
            { "data": "precio", "width": "10%", title: "Precio" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/RegistroMantenimiento/Upsert/${data}" class="btn btn-sm bg-blue-600 text-white rounded px-3 py-1 hover:bg-blue-700" title="Editar">
                            <i class="bi bi-pencil-square"></i> Editar
                        </a>`;
                },
                "width": "15%", title: "Acciones"
            }
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        }
    });
}
