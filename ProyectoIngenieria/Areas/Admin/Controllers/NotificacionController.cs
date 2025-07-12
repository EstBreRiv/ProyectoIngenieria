using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ProyectoIngenieria.Utilities.RolesUsuario.Role_Admin)]

    // Controlador para gestionar las notificaciones generadas para la revision vehicular
    // Permite crear, editar, listar y eliminar notificaciones
    public class NotificacionController : Controller
    {
        // Inyección de dependencias del unit of work
        private readonly IUnitOfWork _unitOfWork;

        // Constructor que recibe el unit of work
        public NotificacionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Acciones del controlador
        // Index: Muestra la vista de notificaciones
        public IActionResult Index()
        {
            return View(); // sin pasarle modelo, porque DataTable lo pide por AJAX
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var notificaciones = _unitOfWork.Notificacion
                .GetAll(includeProperties: "Vehiculo")
                .OrderByDescending(n => n.Fecha)
                .Select(n => new
                {
                    n.Id,
                    n.Titulo,
                    n.Descripcion,
                    Fecha = n.Fecha.ToString("yyyy-MM-dd"),
                    Placa = n.Vehiculo.Placa,
                    Modelo = n.Vehiculo.Modelo,
                    Leida = n.Leida
                })
                .ToList();

            return Json(new { data = notificaciones });
        }

        //HayNotificaciones: Revisa si existen notificaciones nuevas
        [HttpGet]
        public IActionResult HayNotificaciones()
        {
            var sinLeer = _unitOfWork.Notificacion.GetAll()
                .Where(n => !n.Leida)
                .ToList();

            return Json(new { hay = sinLeer.Any(), count = sinLeer.Count });
        }

        [HttpPost]
        public IActionResult MarcarTodasComoLeidas()
        {
            var noLeidas = _unitOfWork.Notificacion.GetAll()
                .Where(n => !n.Leida)
                .ToList();

            foreach (var noti in noLeidas)
            {
                noti.Leida = true;
            }

            _unitOfWork.Save();
            return Ok();
        }

        //Elimina las notificaciones marcando el checkbox
        [HttpPost]
        public IActionResult EliminarSeleccionadas([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any()) return BadRequest();

            foreach (var id in ids)
            {
                var noti = _unitOfWork.Notificacion.Get(n => n.Id == id);
                if (noti != null) _unitOfWork.Notificacion.Remove(noti);
            }

            _unitOfWork.Save();
            return Ok();
        }
    }
}
