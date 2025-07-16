using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoIngenieria.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ProyectoIngenieria.Utilities.RolesUsuario.Role_Admin)]

    public class RegistroMantenimientoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistroMantenimientoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Vista principal
        [HttpGet]
        public IActionResult Index(int id)
        {
            ViewBag.VehiculoId = id; // Pasa el ID a la vista para que el JS lo use
            return View();
        }

        // Mostrar todos los registros (para DataTables)
        [HttpGet]
        public IActionResult GetAll(int? id, DateOnly? fechaInicio, DateOnly? fechaFin)
        {
            var query = _unitOfWork.RegistroMantenimiento.GetAll(includeProperties: "Vehiculo,Vehiculo.Marca")
            .Where(m =>
                (!id.HasValue || id == 0 || m.VehiculoId == id) &&
                (!fechaInicio.HasValue || m.Fecha >= fechaInicio.Value) &&
                (!fechaFin.HasValue || m.Fecha <= fechaFin.Value)
            );

            var mantenimientos = query
                .Select(m => new
                {
                    m.Id,
                    modelo = m.Vehiculo.Modelo,
                    marca = m.Vehiculo.Marca.NombreMarca,
                    placa = m.Vehiculo.Placa,
                    m.Descripcion,
                    Fecha = m.Fecha.ToString("yyyy-MM-dd"),
                    Precio = m.Precio.ToString("C2", new System.Globalization.CultureInfo("es-CR"))
                }).ToList();

            return Json(new { data = mantenimientos });
        }


        // Crear / Editar
        public IActionResult Upsert(int? id, int? vehiculoId)
        {
            var registro = new RegistroMantenimiento
            {
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                VehiculoId = vehiculoId ?? 0
            };

            List<int> repuestosSeleccionados = new();
            List<OperadorMantenimiento> detallesOperadores = new();

            if (id != null && id != 0)
            {
                registro = _unitOfWork.RegistroMantenimiento.Get(r => r.Id == id);
                if (registro == null)
                    return NotFound();


                repuestosSeleccionados = _unitOfWork.RepuestosMantenimiento
                    .GetAll(rm => rm.RegistroMantenimientoId == id)
                    .Select(rm => rm.CatalogoRepuestoId)
                    .ToList();

                detallesOperadores = _unitOfWork.OperadorMantenimiento
                   .GetAll(o => o.RegistroMantenimientoId == id)
                   .Select(o => new OperadorMantenimiento
                       {
                       Id = o.Id,   
                       OperadorCedula = o.OperadorCedula,
                           HorasTrabajo = o.HorasTrabajo,
                           CatalogoMantenimientoId = o.CatalogoMantenimientoId
                       }).ToList();
                    }

            var viewModel = new RegistroMantenimientoVM
            {
                RegistroMantenimiento = registro,
                RepuestosSeleccionados = repuestosSeleccionados,
                DetallesOperadores = detallesOperadores,

                ListaRepuestos = _unitOfWork.Repuesto.GetAll().Where(x => x.Descripcion != "Eliminado").Select(r => new SelectListItem
                {
                    Text = r.Nombre,
                    Value = r.Id.ToString()
                }),

                //RegistroMantenimiento = id == null ? new RegistroMantenimiento() : _unitOfWork.RegistroMantenimiento.Get(m => m.Id == id),

                //Solo permite utilizar los vehiculos activos y que no sean de tipo "Automovil"
                ListaVehiculos = _unitOfWork.Vehiculo
                .GetAll(v => v.Estado == "Activo" && v.TipoVehiculoId != 2)
                .Select(v => new SelectListItem
                {
                    Text = v.Modelo + " - " + v.Placa,
                    Value = v.Id.ToString()
                }),
                ListaCatalogoMantenimiento = _unitOfWork.CatalogoMantenimiento.GetAll().Where(x => x.Descripcion != "Eliminado").Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                }),
                ListaOperadores = _unitOfWork.Operador.GetAll(o => o.TipoColaborador != "Inactivo").Select(o => new SelectListItem
                {
                    Text = o.Nombre,
                    Value = o.Cedula.ToString()
                }),
            };

            if (id != null && viewModel.RegistroMantenimiento == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // POST: Guardar mantenimiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(RegistroMantenimientoVM viewModel)
        {

            if (viewModel.DetallesOperadores == null || viewModel.DetallesOperadores.Count == 0)
            {
                ModelState.AddModelError("DetallesOperadores", "Debe seleccionar al menos un operador.");
            }

            if (viewModel.RegistroMantenimiento.Precio <= 0)
            {
                ModelState.AddModelError("RegistroMantenimiento.Precio", "El precio debe ser mayor a cero.");
            }

            if (viewModel.DetallesOperadores != null)
            {

                foreach (var detalle in viewModel.DetallesOperadores)
                {
                    if (detalle.HorasTrabajo <= 0)
                    {
                        ModelState.AddModelError("DetallesOperadores", "Las horas de trabajo deben ser mayores a cero.");
                    }
                }
            }

            if (viewModel.RepuestosSeleccionados == null || viewModel.RepuestosSeleccionados.Count == 0)
            {
                ModelState.AddModelError("RepuestosSeleccionados", "Debe seleccionar al menos un repuesto.");
            }

            if (!ModelState.IsValid)
            {

                // Si hay errores, volver a cargar los combos
                viewModel.ListaVehiculos = _unitOfWork.Vehiculo
                .GetAll(v => v.Estado == "Activo" && v.TipoVehiculoId != 2)
                .Select(v => new SelectListItem
                {
                    Text = v.Modelo + " - " + v.Placa,
                    Value = v.Id.ToString()
                });

                viewModel.ListaCatalogoMantenimiento = _unitOfWork.CatalogoMantenimiento.GetAll().Where(x => x.Descripcion != "Eliminado").Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });

                viewModel.ListaRepuestos = _unitOfWork.Repuesto.GetAll().Where(x => x.Descripcion != "Eliminado").Select(r => new SelectListItem
                {
                    Text = r.Nombre,
                    Value = r.Id.ToString()
                });

                viewModel.ListaOperadores = _unitOfWork.Operador.GetAll().Where(x => x.TipoColaborador != "Inactivo").Select(o => new SelectListItem
                {
                    Text = o.Nombre,
                    Value = o.Cedula.ToString()
                });

                return View(viewModel);
            }

            if (viewModel.RegistroMantenimiento.Id == 0)
            {
                _unitOfWork.RegistroMantenimiento.Add(viewModel.RegistroMantenimiento);
                _unitOfWork.Save();

                // Reutiliza los métodos existentes para guardar relaciones
                registroProductos(viewModel);
                registroOperadorMantenimiento(viewModel);

                TempData["success"] = "Mantenimiento creado exitosamente";
            }
            else
            {
                _unitOfWork.RegistroMantenimiento.Update(viewModel.RegistroMantenimiento);
                _unitOfWork.Save();

                // Eliminar registros anteriores y registrar nuevos
                var mantenimientoId = viewModel.RegistroMantenimiento.Id;

                var repuestosAntiguos = _unitOfWork.RepuestosMantenimiento
                    .GetAll(r => r.RegistroMantenimientoId == mantenimientoId).ToList();

                foreach (var r in repuestosAntiguos)
                    _unitOfWork.RepuestosMantenimiento.Remove(r);

                var operadoresAntiguos = _unitOfWork.OperadorMantenimiento
                    .GetAll(o => o.RegistroMantenimientoId == mantenimientoId).ToList();

                foreach (var o in operadoresAntiguos)
                    _unitOfWork.OperadorMantenimiento.Remove(o);

                _unitOfWork.Save();

                // Guardar nuevos
                foreach (var repuestoId in viewModel.RepuestosSeleccionados)
                {
                    _unitOfWork.RepuestosMantenimiento.Add(new RepuestosMantenimiento
                    {
                        CatalogoRepuestoId = repuestoId,
                        RegistroMantenimientoId = mantenimientoId
                    });
                }

                foreach (var detalle in viewModel.DetallesOperadores)
                {
                    _unitOfWork.OperadorMantenimiento.Add(new OperadorMantenimiento
                    {
                        HorasTrabajo = detalle.HorasTrabajo,
                        OperadorCedula = detalle.OperadorCedula,
                        CatalogoMantenimientoId = detalle.CatalogoMantenimientoId,
                        RegistroMantenimientoId = mantenimientoId
                    });
                }

                _unitOfWork.Save();

                TempData["success"] = "Mantenimiento actualizado exitosamente";
            }

            return RedirectToAction("Index", new { id = viewModel.RegistroMantenimiento.VehiculoId });
        }

        //Metodo que permite guardar los repuestos seleccionados en el mantenimiento
        //Recibe por parametros el view model que contiene los repuestos seleccionados
        [HttpPost]
        public IActionResult registroProductos(RegistroMantenimientoVM viewModel)
        {

            if (ModelState.IsValid)
            {
                if (viewModel.RepuestosSeleccionados == null || viewModel.RepuestosSeleccionados.Count == 0)
                {
                    // Si no hay repuestos seleccionados, retornar un mensaje de error
                    return Json(new { success = false, message = "Debe seleccionar al menos un repuesto." });
                }

                if (viewModel.RepuestosSeleccionados.Count == 0)
                {
                    return Json(new { success = false, message = "Debe seleccionar al menos un repuesto." });
                }

                //Obtiene el registro mas reciente de mantenimiento
                var ultimoMantenimienro = _unitOfWork.RegistroMantenimiento.
                    GetAll().OrderByDescending(m => m.Id).FirstOrDefault();


                // Guardar los repuestos seleccionados
                foreach (var repuestoId in viewModel.RepuestosSeleccionados)
                {
                    var repuestoMantenimiento = new RepuestosMantenimiento
                    {
                        CatalogoRepuestoId = repuestoId,
                        RegistroMantenimientoId = ultimoMantenimienro.Id
                    };
                    _unitOfWork.RepuestosMantenimiento.Add(repuestoMantenimiento);
                }
                _unitOfWork.Save();
            }

            return Json(new { success = true, message = "Se guardaron correctamente la lista de productos" });
        }

        public IActionResult registroOperadorMantenimiento(RegistroMantenimientoVM registroMantenimientoVM)
        {

            if (ModelState.IsValid)
            {

                if (registroMantenimientoVM.DetallesOperadores == null)
                {
                    // Si no hay operadores seleccionados, retornar un mensaje de error
                    return Json(new { success = false, message = "Debe seleccionar al menos un operador." });
                }

                if (registroMantenimientoVM.DetallesOperadores.Count == 0)
                {
                    return Json(new { success = false, message = "Debe seleccionar al menos un operador." });

                }

                //Obtiene el registro mas reciente de mantenimiento
                var ultimoMantenimienro = _unitOfWork.RegistroMantenimiento.
                    GetAll().OrderByDescending(m => m.Id).FirstOrDefault();

                foreach (var detalle in registroMantenimientoVM.DetallesOperadores)
                {

                    var operadorMantenimiento = new OperadorMantenimiento
                    {
                        HorasTrabajo = detalle.HorasTrabajo,
                        OperadorCedula = detalle.OperadorCedula,
                        CatalogoMantenimientoId = detalle.CatalogoMantenimientoId,
                        RegistroMantenimientoId = ultimoMantenimienro.Id
                    };
                    _unitOfWork.OperadorMantenimiento.Add(operadorMantenimiento);
                }
                _unitOfWork.Save();
                return Json(new { success = true, message = "Operadores registrados correctamente" });
            }
            return Json(new { success = true, message = "Operador registrado correctamente" });
        }

        // Ver detalles de un mantenimiento incluyendo sus productos
        [HttpGet]
        public IActionResult Details(int id)
        {
            var mantenimiento = _unitOfWork.RegistroMantenimiento.Get(m => m.Id == id);
            if (mantenimiento == null)
            {
                return NotFound();
            }

            var vehiculo = _unitOfWork.Vehiculo.Get(v => v.Id == mantenimiento.VehiculoId);
            if (vehiculo == null)
            {
                return NotFound();
            }

            var vehiculoMarca = _unitOfWork.Marca.Get(m => m.Id == vehiculo.MarcaId);
            if (vehiculoMarca == null)
            {
                return NotFound();
            }
            vehiculo.Marca = vehiculoMarca;

            var registroMantenimientoVM = new RegistroMantenimientoVM
            {
                RegistroMantenimiento = mantenimiento,

                vehiculo = vehiculo,

                nombresProductos = _unitOfWork.RepuestosMantenimiento
                    .GetAll(rm => rm.RegistroMantenimientoId == id)
                    .Select(rm => _unitOfWork.Repuesto.Get(r => r.Id == rm.CatalogoRepuestoId).Nombre)
                    .ToList(),

                DetallesOperadores = _unitOfWork.OperadorMantenimiento
                    .GetAll(o => o.RegistroMantenimientoId == id)
                    .ToList(),

                ListaOperadores = _unitOfWork.Operador.GetAll().Select(o => new SelectListItem
                {
                    Text = o.Nombre,
                    Value = o.Cedula.ToString()
                }),

                ListaCatalogoMantenimiento = _unitOfWork.CatalogoMantenimiento.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                })
            };

            return View(registroMantenimientoVM);
        }


        // Historial por vehículo
        public IActionResult Historial(int vehiculoId)
        {
            var historial = _unitOfWork.RegistroMantenimiento.GetAll(
                m => m.VehiculoId == vehiculoId,
                includeProperties: "CatalogoMantenimiento,Vehiculo"
            );

            ViewBag.VehiculoId = vehiculoId;
            return View(historial);
        }
    }
}