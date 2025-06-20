using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Controllers
{
    public class RegistroMantenimientoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistroMantenimientoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Vista principal
        public IActionResult Index()
        {
            return View();
        }

        // Mostrar todos los registros (para DataTables)
        public IActionResult GetAll()
        {
            var mantenimientos = _unitOfWork.RegistroMantenimiento.GetAll()
                .Select(m => new
                {
                    m.Id,
                    VehiculoModelo = _unitOfWork.Vehiculo.Get(x => x.Id == m.VehiculoId).Modelo,
                    m.Descripcion,
                    CatalogoMantenimiento = _unitOfWork.CatalogoMantenimiento.Get(x => x.Id == m.CatalogoMantenimientoId).Nombre,
                    Fecha = m.Fecha.ToString("dd/MM/yyyy"),
                    Precio = m.Precio.ToString("C2", new System.Globalization.CultureInfo("es-CR"))
                });

            return Json(new { data = mantenimientos });
        }


        // Crear / Editar
        public IActionResult Upsert(int? id)
        {
            var viewModel = new RegistroMantenimientoVM
            {
                ListaRepuestos = _unitOfWork.Repuesto.GetAll().Select(r => new SelectListItem
                {
                    Text = r.Nombre,
                    Value = r.Id.ToString()
                }),

                RegistroMantenimiento = id == null ? new RegistroMantenimiento() : _unitOfWork.RegistroMantenimiento.Get(m => m.Id == id),

                ListaVehiculos = _unitOfWork.Vehiculo.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Modelo + " - " + v.Placa,
                    Value = v.Id.ToString()
                }),
                ListaCatalogoMantenimiento = _unitOfWork.CatalogoMantenimiento.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                }),
                ListaOperadores = _unitOfWork.Operador.GetAll().Select(o => new SelectListItem
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
            if (ModelState.IsValid)
            {
                if (viewModel.RegistroMantenimiento.Id == 0)
                {
                    _unitOfWork.RegistroMantenimiento.Add(viewModel.RegistroMantenimiento);
                    TempData["success"] = "Mantenimiento creado exitosamente";
                }
                else
                {
                    _unitOfWork.RegistroMantenimiento.Update(viewModel.RegistroMantenimiento);
                    TempData["success"] = "Mantenimiento actualizado exitosamente";
                }

                _unitOfWork.Save();

                registroProductos(viewModel);

                return RedirectToAction(nameof(Index));
            }

            // Si hay errores, volver a cargar los combos
            viewModel.ListaVehiculos = _unitOfWork.Vehiculo.GetAll().Select(v => new SelectListItem
            {
                Text = v.Modelo + " - " + v.Placa,
                Value = v.Id.ToString()
            });

            viewModel.ListaCatalogoMantenimiento = _unitOfWork.CatalogoMantenimiento.GetAll().Select(c => new SelectListItem
            {
                Text = c.Nombre,
                Value = c.Id.ToString()
            });

            return View(viewModel);
        }

        //Metodo que permite guardar los repuestos seleccionados en el mantenimiento
        //Recibe por parametros el view model que contiene los repuestos seleccionados
        [HttpPost]
        public IActionResult registroProductos(RegistroMantenimientoVM viewModel)
        {

            if (ModelState.IsValid)
            {
                if (viewModel.RepuestosSeleccionados.Count == 0) { 
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

        // Ver detalles de un mantenimiento incluyendo sus productos
        [HttpGet]
        public IActionResult Details(int id)
        {

            var mantenimiento = _unitOfWork.RegistroMantenimiento.Get(m => m.Id == id);

            if (mantenimiento == null)
            {
                return NotFound();
            }
            RegistroMantenimientoVM registroMantenimientoVM = new RegistroMantenimientoVM
            {
                RegistroMantenimiento = mantenimiento,

                nombreVehiculo = _unitOfWork.Vehiculo.Get(v => v.Id == mantenimiento.VehiculoId).Marca + "  " +
                _unitOfWork.Vehiculo.Get(v => v.Id == mantenimiento.VehiculoId).Modelo + " - " +
                                  _unitOfWork.Vehiculo.Get(v => v.Id == mantenimiento.VehiculoId).Placa,

                nombreOperador = _unitOfWork.Operador.Get(o => o.Cedula == mantenimiento.OperadorCedula).Nombre,

                tipoMantenimiento = _unitOfWork.CatalogoMantenimiento.Get(c => c.Id == mantenimiento.CatalogoMantenimientoId).Nombre,
            };

            // Obtener los nombres de los productos asociados al mantenimiento
            var listaRepuestos = _unitOfWork.RepuestosMantenimiento.GetAll(rm => rm.RegistroMantenimientoId == id);

            foreach (var repuesto in listaRepuestos)
            {
                var nombre = _unitOfWork.Repuesto.Get(r => r.Id == repuesto.CatalogoRepuestoId).Nombre;
                registroMantenimientoVM.nombresProductos.Add(nombre); 
            }
            if (mantenimiento == null)
            {
                return NotFound();
            }

           

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