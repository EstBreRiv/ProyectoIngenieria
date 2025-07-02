using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Controllers
{
    public class RegistroOperadoresController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistroOperadoresController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll(int? id, DateOnly? fechaInicio, DateOnly? fechaFin)
        {
            var registros = _unitOfWork.RegistroOperadores.GetAll(
                r =>
                    (!id.HasValue || id == 0 || r.VehiculoId == id) &&
                    (!fechaInicio.HasValue || DateOnly.FromDateTime(r.FechaInicio) >= fechaInicio.Value) &&
                    (!fechaFin.HasValue || (r.FechaFin != DateTime.MinValue && DateOnly.FromDateTime(r.FechaFin) <= fechaFin.Value))
            );

            var data = registros.Select(r =>
            {
                var vehiculo = _unitOfWork.Vehiculo.Get(v => v.Id == r.VehiculoId);
                var operador = _unitOfWork.Operador.Get(o => o.Cedula == r.OperadorCedula);

                return new
                {
                    id = r.Id,
                    fechaInicio = r.FechaInicio.ToString("yyyy-MM-dd"),
                    fechaFin = r.FechaFin != DateTime.MinValue ? r.FechaFin.ToString("yyyy-MM-dd") : "",
                    nombreVehiculo = vehiculo != null ? vehiculo.Modelo: "",
                    placaVehiculo = vehiculo != null ? vehiculo.Placa : "",
                    nombreOperador = operador != null ? operador.Nombre : ""
                };
            }).ToList();

            return Json(new { data });
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            // Obtener la entidad si se trata de una edición
            RegistroOperadore registro = id == null ? new RegistroOperadore() : _unitOfWork.RegistroOperadores.Get(x => x.Id == id);

            if (id != null && registro == null)
                return NotFound();

            // Obtener vehículos activos + el actual si no está activo
            var vehiculos = _unitOfWork.Vehiculo.GetAll(v => v.Estado == "Activo").ToList();
            if (registro.VehiculoId != 0 && !vehiculos.Any(v => v.Id == registro.VehiculoId))
            {
                var vehiculoActual = _unitOfWork.Vehiculo.Get(v => v.Id == registro.VehiculoId);
                if (vehiculoActual != null)
                {
                    vehiculos.Add(vehiculoActual);
                }
            }

            var listaVehiculos = new List<SelectListItem>
    {
        new SelectListItem { Text = "Seleccione un vehículo", Value = "0" }
    };
            listaVehiculos.AddRange(vehiculos.Select(v => new SelectListItem
            {
                Text = $"{v.Modelo} - {v.Placa}",
                Value = v.Id.ToString()
            }));

            // Obtener operadores
            var operadores = _unitOfWork.Operador.GetAll().ToList();
            var listaOperadores = new List<SelectListItem>
    {
        new SelectListItem { Text = "Seleccione un operador", Value = "" }
    };
            listaOperadores.AddRange(operadores.Select(o => new SelectListItem
            {
                Text = o.Nombre,
                Value = o.Cedula.ToString()
            }));

            var viewModel = new RegistroOperadoresVM
            {
                RegistroOperador = registro,
                ListaVehiculos = listaVehiculos,
                ListaOperadores = listaOperadores,
                RegistroOperadores = new List<SelectListItem>() // opcional, si lo usás en algún lugar
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(RegistroOperadoresVM viewModel)
        {
            // Validación de fechas
            if (viewModel.RegistroOperador.FechaFin != DateTime.MinValue &&
                viewModel.RegistroOperador.FechaFin < viewModel.RegistroOperador.FechaInicio)
            {
                ModelState.AddModelError("RegistroOperador.FechaFin", "La fecha de finalización no puede ser anterior a la fecha de inicio.");
            }

            if (ModelState.IsValid)
            {
                if (viewModel.RegistroOperador.Id == 0)
                {
                    _unitOfWork.RegistroOperadores.Add(viewModel.RegistroOperador);
                    TempData["success"] = "Asignación creada correctamente";
                }
                else
                {
                    _unitOfWork.RegistroOperadores.Update(viewModel.RegistroOperador);
                    TempData["success"] = "Asignación actualizada correctamente";
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            // Si falla la validación, recargar listas
            viewModel.ListaVehiculos = _unitOfWork.Vehiculo
                .GetAll(v => v.Estado == "Activo")
                .Select(v => new SelectListItem
                {
                    Text = v.Modelo + " - " + v.Placa,
                    Value = v.Id.ToString()
                });

            viewModel.ListaOperadores = _unitOfWork.Operador
                .GetAll()
                .Select(o => new SelectListItem
                {
                    Text = o.Nombre,
                    Value = o.Cedula.ToString()
                });

            viewModel.RegistroOperadores = _unitOfWork.RegistroOperadores
                .GetAll()
                .Select(r => new SelectListItem
                {
                    Text = $"Op: {r.OperadorCedula} - Veh: {r.VehiculoId}",
                    Value = r.Id.ToString()
                });

            return View(viewModel);
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.RegistroOperadores.Get(x => x.Id == id);
            if (obj == null)
                return Json(new { success = false, message = "No se encontró el registro" });

            _unitOfWork.RegistroOperadores.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Registro eliminado correctamente" });
        }
    }
}
