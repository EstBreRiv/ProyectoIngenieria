using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;
using System.Linq;

namespace ProyectoIngenieria.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehiculoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var vehiculos = _unitOfWork.Vehiculo.GetAll()
            
                .Select(v => new
                {
                    v.Id,
                    v.Modelo,
                    v.Estado,
                    v.Descripcion,
                    v.Placa,
                    v.Tipo,
                    EmpresaNombre = _unitOfWork.Empresa.Get(x => x.Id == v.EmpresaId).Nombre
                })
                .ToList();
            return Json(new { data = vehiculos });
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            // Cargar empresas para el dropdown
            var empresas = _unitOfWork.Empresa.GetAll();
            ViewBag.EmpresaList = new SelectList(empresas, "Id", "Nombre");

            VehiculoVM vehiculoVM = new()
            {
                Vehiculo = new Vehiculo(),
                VehiculosList = _unitOfWork.Vehiculo.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Modelo,
                    Value = i.Id.ToString()
                }).ToList()
            };

            if (id == null || id == 0)
            {
                // Crear nuevo - estado Activo por defecto
                vehiculoVM.Vehiculo.Estado = "Activo";
                return View(vehiculoVM);
            }
            else
            {
                // Editar existente
                vehiculoVM.Vehiculo = _unitOfWork.Vehiculo.Get(u => u.Id == id);
                if (vehiculoVM.Vehiculo == null)
                {
                    return NotFound();
                }
                return View(vehiculoVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(VehiculoVM vehiculoVM)
        {
            if (ModelState.IsValid)
            {
                // Forzar estado como Activo
                vehiculoVM.Vehiculo.Estado = "Activo";

                // Manejar campo de placa
                var tienePlaca = Request.Form["mostrarPlaca"].Count > 0;
                if (!tienePlaca)
                {
                    vehiculoVM.Vehiculo.Placa = null;
                }

                if (vehiculoVM.Vehiculo.Id == 0)
                {
                    _unitOfWork.Vehiculo.Add(vehiculoVM.Vehiculo);
                }
                else
                {
                    _unitOfWork.Vehiculo.Update(vehiculoVM.Vehiculo);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            // Recargar empresas si hay error de validación
            var empresas = _unitOfWork.Empresa.GetAll();
            ViewBag.EmpresaList = new SelectList(empresas, "Id", "Nombre");
            return View(vehiculoVM);
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }

                var vehiculo = _unitOfWork.Vehiculo.Get(u => u.Id == id);
                if (vehiculo == null)
                {
                    return NotFound();
                }

                //soft delete
                vehiculo.Estado = "Inactivo"; // Cambiar estado a Inactivo
                //operador.VehiculoId = 0; // Desasociar operador del vehículo
                _unitOfWork.Vehiculo.Update(vehiculo);
                //_unitOfWork.Operador.Update(operador);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Se ha inactivado exitosamente" });
            }
            catch
            {
                return Json(new { success = false, message = "No se pudo inactivar" });
            }
        }

        [HttpGet]
        public IActionResult DetalleVehiculo(int id)
        {
            var vehiculo = _unitOfWork.Vehiculo.Get(v => v.Id == id, includeProperties: "Empresa");

            var vehiculoVM = new VehiculoVM
            {
                Vehiculo = vehiculo
            };

            return View(vehiculoVM);
        }

        [HttpPost]
        public IActionResult Activar(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }

                var vehiculo = _unitOfWork.Vehiculo.Get(u => u.Id == id);
                if (vehiculo == null)
                {
                    return NotFound();
                }

                vehiculo.Estado = "Activo";
                _unitOfWork.Vehiculo.Update(vehiculo);
                _unitOfWork.Save();

                return Json(new { success = true, message = "Se ha activado exitosamente" });
            }
            catch
            {
                return Json(new { success = false, message = "No se pudo activar" });
            }
        }

    }
}