using Microsoft.AspNetCore.Mvc;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Controllers
{
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
            var notificaciones = _unitOfWork.Notificacion
                .GetAll(includeProperties: "Vehiculo")
                .OrderByDescending(n => n.Fecha)
                .ToList();

            // Marcar como leídas
            foreach (var notificacion in notificaciones.Where(n => !n.Leida))
            {
                notificacion.Leida = true;
            }

            _unitOfWork.Save();

            return View(notificaciones);
        }

        //HayNotificaciones: Revisa si existen notificaciones nuevas
        [HttpGet]
        public IActionResult HayNotificaciones()
        {
            var hayNoLeidas = _unitOfWork.Notificacion.GetAll()
                .Any(n => !n.Leida);

            return Json(new { hay = hayNoLeidas });
        }
    }
}
