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
                //RegistroMantenimiento = id == null ? new RegistroMantenimiento() : _unitOfWork.RegistroMantenimiento.Get(m => m.Id == id),
                //ListaVehiculos = _unitOfWork.Vehiculo.GetAll().Select(v => new SelectListItem
                //{
                //    Text = v.Modelo + " - " + v.Placa,
                //    Value = v.Id.ToString()
                //}),
                //ListaCatalogoMantenimiento = _unitOfWork.CatalogoMantenimiento.GetAll().Select(c => new SelectListItem
                //{
                //    Text = c.Nombre,
                //    Value = c.Id.ToString()
                //}),
                //ListaOperadores = _unitOfWork.Operador.GetAll().Select(o => new SelectListItem
                //{
                //    Text = o.Nombre,
                //    Value = o.Cedula.ToString()
                //}),
                //ListaRepuestos = _unitOfWork.Repuesto.GetAll().Select(r => new SelectListItem
                //{
                //    Text = r.Nombre,
                //    Value = r.Id.ToString()
                //})
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
                return RedirectToAction(nameof(Index));
            }

            //// Si hay errores, volver a cargar los combos
            //viewModel.ListaVehiculos = _unitOfWork.Vehiculo.GetAll().Select(v => new SelectListItem
            //{
            //    Text = v.Modelo + " - " + v.Placa,
            //    Value = v.Id.ToString()
            //});

            //viewModel.ListaCatalogoMantenimiento = _unitOfWork.CatalogoMantenimiento.GetAll().Select(c => new SelectListItem
            //{
            //    Text = c.Nombre,
            //    Value = c.Id.ToString()
            //});

            return View(viewModel);
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