using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;
using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Controllers
{
    public class OperadorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OperadorController(IUnitOfWork unitOfWork)
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
            var operadores = _unitOfWork.Operador.GetAll();
            return Json(new { data = operadores });
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            // Cargar vehiculos para el dropdown
            var vehiculos = _unitOfWork.Vehiculo.GetAll();
            ViewBag.VehiculosList = new SelectList(vehiculos, "Id", "Modelo");

            OperadorVM operadorVM = new()
            {
                Operador = new Operador(),
                OperadoresList = _unitOfWork.Operador.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Cedula.ToString()
                }).ToList()
            };

            if (id == null || id == 0)
            {
                // Create
                return View(operadorVM);
            }
            else
            {
                // Update
                operadorVM.Operador = _unitOfWork.Operador.Get(u => u.Cedula == id);
                if (operadorVM.Operador == null)
                {
                    return NotFound();
                }
                return View(operadorVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(OperadorVM operadorVM)
        {

            if (ModelState.IsValid)
            {
                if (operadorVM.Operador.Cedula == 0)
                {
                    _unitOfWork.Operador.Add(operadorVM.Operador);
                }
                else
                {
                    _unitOfWork.Operador.Update(operadorVM.Operador);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(operadorVM);
        }

    }
}