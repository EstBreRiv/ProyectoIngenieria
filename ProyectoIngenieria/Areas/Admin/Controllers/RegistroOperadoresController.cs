using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ProyectoIngenieria.Utilities.RolesUsuario.Role_Admin)]

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
            var registros = _unitOfWork.RegistroOperadores.GetAll(includeProperties: "Vehiculo,Vehiculo.Marca,OperadorCedulaNavigation").Where(r =>
                    (!id.HasValue || id == 0 || r.VehiculoId == id) &&
                    (!fechaInicio.HasValue || DateOnly.FromDateTime(r.FechaInicio) >= fechaInicio.Value) &&
                    (!fechaFin.HasValue || r.FechaFin != DateTime.MinValue && DateOnly.FromDateTime(r.FechaFin) <= fechaFin.Value)
            );

            var data = registros.Select(r => new
            {
                r.Id,
                fechaInicio = r.FechaInicio.ToString("yyyy-MM-dd"),
                fechaFin = r.FechaFin != DateTime.MinValue ? r.FechaFin.ToString("yyyy-MM-dd") : "",
                marca = r.Vehiculo.Marca.NombreMarca,
                modelo = r.Vehiculo.Modelo,
                placa = r.Vehiculo.Placa,
                nombreOperador = r.OperadorCedulaNavigation.Nombre
            }).ToList();

            return Json(new { data });
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            // Obtener la entidad si se trata de una edición
            RegistroOperadore registro = id == null
                ? new RegistroOperadore
                {
                    FechaInicio = DateTime.Today,
                    FechaFin = DateTime.Today // Pone la fecha actual en ambos, para evitar errores de validación
                }
                : _unitOfWork.RegistroOperadores.Get(x => x.Id == id);

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

            // Obtener operadores que tengan tipo de colaborador distinto de Inactivo
            var operadores = _unitOfWork.Operador.GetAll(o => o.TipoColaborador != "Inactivo").ToList();

            var listaOperadores = new List<SelectListItem>
    {
        new SelectListItem { Text = "Seleccione un colaborador", Value = "" }
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
                RegistroOperadores = new List<SelectListItem>() // opcional
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

            //alidación de campos requeridos
            if (viewModel.RegistroOperador.VehiculoId == 0)
            {
                ModelState.AddModelError("RegistroOperador.VehiculoId", "Debe seleccionar un vehículo.");
            }

            if (viewModel.RegistroOperador.OperadorCedula == 0)
            {
                ModelState.AddModelError("RegistroOperador.OperadorCedula", "Debe seleccionar un operador.");
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
