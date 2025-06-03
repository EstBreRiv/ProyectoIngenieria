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
            var vehiculos = _unitOfWork.Vehiculo.GetAll()
                .Where(v => v.Estado != "Inactivo");
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
                var operadorExistente = _unitOfWork.Operador.Get(u => u.Cedula == operadorVM.Operador.Cedula);

                if (operadorExistente == null)
                {
                    // Nuevo operador
                    _unitOfWork.Operador.Add(operadorVM.Operador);
                }
                else
                {
                    // Actualizar campos manualmente sobre la instancia ya trackeada
                    operadorExistente.Nombre = operadorVM.Operador.Nombre;
                    operadorExistente.VehiculoId = operadorVM.Operador.VehiculoId;

                    // Ya está siendo trackeado, así que no uses Update()
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            // Recargar lista de vehículos en caso de error
            var vehiculos = _unitOfWork.Vehiculo.GetAll().Where(v => v.Estado != "Inactivo");
            ViewBag.VehiculosList = new SelectList(vehiculos, "Id", "Modelo");

            return View(operadorVM);
        }

        [HttpGet]
        public IActionResult DocumentoOperador(int id)
        {
            var operador = _unitOfWork.Operador.GetAll().Where(o => o.Cedula == id);
            OperadorVM operadorVM = new OperadorVM();
            operadorVM.Operador = _unitOfWork.Operador.Get(u => u.Cedula == id);

            if (operador == null)
            {
                return NotFound();
            }

            var documentos = _unitOfWork.DocumentoOperador.GetAll().Where(d => d.OperadorCedula == id);
            operadorVM.Operador.DocumentoOperadors = documentos.ToList();

            return View(operadorVM);
        }

        
        [HttpGet]
        public IActionResult CargarDocumentoOperador()
        {
            DocumentoOperadorVM documentoOperadorVM = new DocumentoOperadorVM();
            return View(documentoOperadorVM);
        }


    }
}