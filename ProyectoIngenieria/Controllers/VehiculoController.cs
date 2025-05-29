using Microsoft.AspNetCore.Mvc;
using ProyectoIngenieria.Repository.Interfaces;

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
            //Se necesita un viewModel para mostrar la empresa
            return Json(new { data = vehiculos });
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Models.Vehiculo vehiculo = new Models.Vehiculo();

            if (id == null || id == 0)
            {
                // Create
                return View(vehiculo);
            }
            else
            {
                // Update
                vehiculo = _unitOfWork.Vehiculo.Get(u => u.Id == id);
                return View(vehiculo);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Models.Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
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
            return View(vehiculo);
        }
    }
}
