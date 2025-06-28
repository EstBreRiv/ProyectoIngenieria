using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;
using System.Linq;

namespace ProyectoIngenieria.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ProyectoIngenieria.Utilities.RolesUsuario.Role_Admin)]
    public class HorasTrabajoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HorasTrabajoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            ViewBag.VehiculoId = id;
            return View();
        }

        [HttpGet]
        public IActionResult GetAll(int id, DateOnly? fechaInicio, DateOnly? fechaFin)
        {
            var horas = _unitOfWork.HorasTrabajo.GetAll(
                h => h.VehiculoId == id &&
                     (!fechaInicio.HasValue || h.Fecha >= fechaInicio.Value) &&
                     (!fechaFin.HasValue || h.Fecha <= fechaFin.Value),
                includeProperties: "LugarTrabajo,TipoTrabajo,Proyecto"
            );

            var data = horas.Select(h => new
            {
                id = h.Id,
                fecha = h.Fecha.ToString("yyyy-MM-dd"),
                horometroInicial = h.HorometroInicial,
                horometroFinal = h.HorometroFinal,
                precioHora = h.PrecioHora,
                totalHoras = h.TotalHoras,
                totalGanancia = h.TotalGanancia,
                lugar = h.LugarTrabajo != null ? h.LugarTrabajo.Nombre : "",
                tipo = h.TipoTrabajo != null ? h.TipoTrabajo.Nombre : "",
                proyecto = h.Proyecto != null ? h.Proyecto.NombreProyecto : ""
            }).ToList();

            return Json(new { data });
        }


        [HttpGet]
        public IActionResult Upsert(int? id, int? vehiculoId)
        {
            var horasTrabajoVM = new HorasTrabajoVM
            {
                HorasTrabajo = new HorasTrabajo
                {
                    Fecha = DateOnly.FromDateTime(DateTime.Today),
                    VehiculoId = vehiculoId ?? 0
                },
                LugarTrabajoList = _unitOfWork.LugarTrabajo.GetAll().Select(l => new SelectListItem
                {
                    Text = l.Nombre,
                    Value = l.Id.ToString()
                }),
                TipoTrabajoList = _unitOfWork.TipoTrabajo.GetAll().Select(t => new SelectListItem
                {
                    Text = t.Nombre,
                    Value = t.Id.ToString()
                }),
                ProyectoList = _unitOfWork.Proyecto.GetAll().Select(p => new SelectListItem
                {
                    Text = p.NombreProyecto,
                    Value = p.Id.ToString()
                })
            };

            if (id != null && id != 0)
            {
                horasTrabajoVM.HorasTrabajo = _unitOfWork.HorasTrabajo.Get(h => h.Id == id);
                if (horasTrabajoVM.HorasTrabajo == null)
                    return NotFound();
            }

            ViewBag.VehiculoId = horasTrabajoVM.HorasTrabajo.VehiculoId;
            return View(horasTrabajoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(HorasTrabajoVM horasVM)
        {
            if (horasVM.HorasTrabajo.VehiculoId <= 0)
                ModelState.AddModelError("HorasTrabajo.VehiculoId", "Vehículo inválido.");

            // Validaciones manuales adicionales
            if (horasVM.HorasTrabajo.HorometroInicial <= 0)
                ModelState.AddModelError("HorasTrabajo.HorometroInicial", "El horómetro inicial debe ser mayor que cero.");

            if (horasVM.HorasTrabajo.HorometroFinal <= 0)
                ModelState.AddModelError("HorasTrabajo.HorometroFinal", "El horómetro final debe ser mayor que cero.");

            if (horasVM.HorasTrabajo.PrecioHora <= 0)
                ModelState.AddModelError("HorasTrabajo.PrecioHora", "El precio por hora debe ser mayor que cero.");

            if (horasVM.HorasTrabajo.HorometroFinal < horasVM.HorasTrabajo.HorometroInicial)
                ModelState.AddModelError("HorasTrabajo.HorometroFinal", "El horómetro final no puede ser menor que el inicial.");

            if (horasVM.HorasTrabajo.LugarTrabajoId == 0)
                ModelState.AddModelError("HorasTrabajo.LugarTrabajoId", "Debe seleccionar un lugar de trabajo.");

            if (horasVM.HorasTrabajo.TipoTrabajoId == 0)
                ModelState.AddModelError("HorasTrabajo.TipoTrabajoId", "Debe seleccionar un tipo de trabajo.");

            if (horasVM.HorasTrabajo.ProyectoId == 0)
                ModelState.AddModelError("HorasTrabajo.ProyectoId", "Debe seleccionar un proyecto.");

            if (!ModelState.IsValid)
            {
                horasVM.LugarTrabajoList = _unitOfWork.LugarTrabajo.GetAll().Select(l => new SelectListItem
                {
                    Text = l.Nombre,
                    Value = l.Id.ToString()
                });

                horasVM.TipoTrabajoList = _unitOfWork.TipoTrabajo.GetAll().Select(t => new SelectListItem
                {
                    Text = t.Nombre,
                    Value = t.Id.ToString()
                });

                horasVM.ProyectoList = _unitOfWork.Proyecto.GetAll().Select(p => new SelectListItem
                {
                    Text = p.NombreProyecto,
                    Value = p.Id.ToString()
                });

                return View(horasVM);
            }

            horasVM.HorasTrabajo.TotalHoras = horasVM.HorasTrabajo.HorometroFinal - horasVM.HorasTrabajo.HorometroInicial;
            horasVM.HorasTrabajo.TotalGanancia = horasVM.HorasTrabajo.TotalHoras * horasVM.HorasTrabajo.PrecioHora;

            if (horasVM.HorasTrabajo.Id == 0)
                _unitOfWork.HorasTrabajo.Add(horasVM.HorasTrabajo);
            else
                _unitOfWork.HorasTrabajo.Update(horasVM.HorasTrabajo);

            _unitOfWork.Save();

            return RedirectToAction("Index", new { id = horasVM.HorasTrabajo.VehiculoId });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var horas = _unitOfWork.HorasTrabajo.Get(h => h.Id == id);
            if (horas == null)
            {
                return Json(new { success = false, message = "No se encontró el registro." });
            }

            _unitOfWork.HorasTrabajo.Remove(horas);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Registro eliminado exitosamente." });
        }
    }
}
