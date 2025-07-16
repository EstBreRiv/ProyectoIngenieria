using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public IActionResult GetAll(int? id, DateOnly? fechaInicio, DateOnly? fechaFin)
        {
            // Si no se proporciona ID o es 0, no se filtra por vehículo
            var horas = _unitOfWork.HorasTrabajo.GetAll(includeProperties: "LugarTrabajo,TipoTrabajo,Proyecto,Vehiculo,Vehiculo.Marca")
            .Where(h => (!id.HasValue || id == 0 || h.VehiculoId == id) &&
                     (!fechaInicio.HasValue || h.Fecha >= fechaInicio.Value) &&
                     (!fechaFin.HasValue || h.Fecha <= fechaFin.Value)
            );

            var data = horas.Select(h => new
            {
                h.Id,
                fecha = h.Fecha.ToString("yyyy-MM-dd"),
                marca = h.Vehiculo.Marca.NombreMarca ?? "",
                modelo = h.Vehiculo.Modelo,
                placa = h.Vehiculo.Placa,
                h.HorometroInicial,
                h.HorometroFinal,
                h.PrecioHora,
                h.TotalHoras,
                h.TotalGanancia,
                lugar = h.LugarTrabajo?.Nombre ?? "",
                tipo = h.TipoTrabajo?.Nombre ?? "",
                proyecto = h.Proyecto?.NombreProyecto ?? ""
            }).ToList();

            return Json(new { data });
        }


        [HttpGet]
        public IActionResult Upsert(int? id, int? vehiculoId)
        {
            var horasTrabajo = new HorasTrabajo
            {
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                VehiculoId = vehiculoId ?? 0
            };

            if (id != null && id != 0)
            {
                horasTrabajo = _unitOfWork.HorasTrabajo.Get(h => h.Id == id);
                if (horasTrabajo == null)
                    return NotFound();
            }

            var horasTrabajoVM = new HorasTrabajoVM
            {
                HorasTrabajo = horasTrabajo,


                LugarTrabajoList = _unitOfWork.LugarTrabajo.GetAll().Where(x => x.Canton != "Eliminado").Select(l => new SelectListItem
                {
                    Text = l.Nombre,
                    Value = l.Id.ToString()
                }),
                TipoTrabajoList = _unitOfWork.TipoTrabajo.GetAll().Where(x => x.Descripcion != "Eliminado").Select(t => new SelectListItem
                {
                    Text = t.Nombre,
                    Value = t.Id.ToString()
                }),
                ProyectoList = _unitOfWork.Proyecto.GetAll().Where(x=>x.Cliente != "Eliminado").Select(p => new SelectListItem
                {
                    Text = p.NombreProyecto,
                    Value = p.Id.ToString()
                }),
                VehiculoList = _unitOfWork.Vehiculo
                .GetAll(v => v.Estado == "Activo" && v.TipoVehiculoId != 2)
                .Select(v => new SelectListItem
                {
                    Text = v.Modelo + " - " + v.Placa,
                    Value = v.Id.ToString()
                })
            };

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

            var registroIdActual = horasVM.HorasTrabajo.Id;

            // Consulta por las fechas anteriores a la fecha ingresada, las ordena de forma descendiente
            // y obtiene el último registro anterior a la fecha ingresada.
            var registroAnterior = _unitOfWork.HorasTrabajo
                .GetAll(h => h.VehiculoId == horasVM.HorasTrabajo.VehiculoId &&
                             h.Fecha < horasVM.HorasTrabajo.Fecha &&
                             h.Id != registroIdActual)
                .OrderByDescending(h => h.Fecha)
                .FirstOrDefault();

            //Valida si existen fechas anteriores y si el horometro proporcionado es menor que el horómetro final del último registro.
            if (registroAnterior != null && horasVM.HorasTrabajo.HorometroInicial < registroAnterior.HorometroFinal)
            {
                //En caso de que el horometro proporcionado sea menor que el horómetro final del último registro,
                //se agrega un error al modelo indicando el último horómetro registrado y la fecha.
                var ultimaFecha = registroAnterior.Fecha.ToString("dd/MM/yyyy");
                ModelState.AddModelError("HorasTrabajo.HorometroInicial",
                    $"El último horómetro registrado para esta máquina es de {registroAnterior.HorometroFinal} en la fecha: {ultimaFecha}.");
            }

            // Consulta por las fechas posteriores a la fecha ingresada, las ordena de forma ascendente
            // y obtiene el primer registro posterior a la fecha ingresada.
            var registroPosterior = _unitOfWork.HorasTrabajo
                .GetAll(h => h.VehiculoId == horasVM.HorasTrabajo.VehiculoId &&
                             h.Fecha > horasVM.HorasTrabajo.Fecha &&
                             h.Id != registroIdActual)
                .OrderBy(h => h.Fecha)
                .FirstOrDefault();

            // Valida si existen fechas posteriores y si el horómetro proporcionado es mayor que el horómetro inicial del primer registro posterior.
            if (registroPosterior != null && horasVM.HorasTrabajo.HorometroFinal > registroPosterior.HorometroInicial)
            {
                // En caso de que el horómetro proporcionado sea mayor que el horómetro inicial del primer registro posterior,
                // se agrega un error al modelo indicando el horómetro inicial del primer registro posterior y la fecha.
                var primeraFecha = registroPosterior.Fecha.ToString("dd/MM/yyyy");
                ModelState.AddModelError("HorasTrabajo.HorometroFinal",
                    $"No se puede registrar ya que se registró un horómetro de {registroPosterior.HorometroInicial} en la fecha: {primeraFecha}.");
            }


            if (!ModelState.IsValid)
            {

                horasVM.LugarTrabajoList = _unitOfWork.LugarTrabajo.GetAll().Where(x => x.Canton != "Eliminado").Select(l => new SelectListItem
                {
                    Text = l.Nombre,
                    Value = l.Id.ToString()
                });

                horasVM.TipoTrabajoList = _unitOfWork.TipoTrabajo.GetAll().Where(x=> x.Descripcion != "Eliminado").Select(t => new SelectListItem
                {
                    Text = t.Nombre,
                    Value = t.Id.ToString()
                });

                horasVM.ProyectoList = _unitOfWork.Proyecto.GetAll().Where(x => x.Cliente != "Eliminado").Select(p => new SelectListItem
                {
                    Text = p.NombreProyecto,
                    Value = p.Id.ToString()
                });

                horasVM.VehiculoList = _unitOfWork.Vehiculo.GetAll().Where(x=>x.TipoVehiculoId != 2).Select(v => new SelectListItem
                {
                    Text = v.Modelo + " - " + v.Placa,
                    Value = v.Id.ToString()
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

        [HttpGet]
        public IActionResult DetalleHorasTrabajo(int id)
        {
            var horasTrabajo = _unitOfWork.HorasTrabajo.Get(h => h.Id == id, includeProperties: "LugarTrabajo,TipoTrabajo,Proyecto");

            var horasTrabajoVM = new HorasTrabajoVM
            {
                HorasTrabajo = horasTrabajo
            };

            return View(horasTrabajoVM);
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
