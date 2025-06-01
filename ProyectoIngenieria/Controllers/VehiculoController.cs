using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var vehiculos = _unitOfWork.Vehiculo.GetAll();
            Console.WriteLine("Vehiculos retrieved: " + vehiculos.Count());
            return Json(new { data = vehiculos });
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            // Cargar empresas para el dropdown
            var empresas = _unitOfWork.Empresa.GetAll();
            ViewBag.EmpresaList = new SelectList(empresas, "Id", "Nombre");

            Models.Vehiculo vehiculo = new();

            if (id == null || id == 0)
            {
                // Crear nuevo - estado Activo por defecto
                vehiculo.Estado = "Activo";
                return View(vehiculo);
            }
            else
            {
                // Editar existente
                vehiculo = _unitOfWork.Vehiculo.Get(u => u.Id == id);
                if (vehiculo == null)
                {
                    return NotFound();
                }
                return View(vehiculo);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Models.Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                // Forzar estado como Activo
                vehiculo.Estado = "Activo";

                // Manejar campo de placa
                var tienePlaca = Request.Form["mostrarPlaca"].Count > 0;
                if (!tienePlaca)
                {
                    vehiculo.Placa = null;
                }

                if (vehiculo.Id == 0)
                {
                    _unitOfWork.Vehiculo.Add(vehiculo);
                }
                else
                {
                    _unitOfWork.Vehiculo.Update(vehiculo);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            // Recargar empresas si hay error de validación
            var empresas = _unitOfWork.Empresa.GetAll();
            ViewBag.EmpresaList = new SelectList(empresas, "Id", "Nombre");
            return View(vehiculo);
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
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
            _unitOfWork.Vehiculo.Update(vehiculo);
            return View(vehiculo);
        }
    }
}