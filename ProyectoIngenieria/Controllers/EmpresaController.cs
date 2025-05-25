using Microsoft.AspNetCore.Mvc;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmpresaController(IUnitOfWork unitOfWork)
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
            var empresas = _unitOfWork.Empresa.GetAll();
            return Json(new { data = empresas });
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Models.Empresa empresa = new Models.Empresa();

            if (id == null || id == 0)
            {
                // Create
                return View(empresa);
            }
            else
            {
                // Update
                empresa = _unitOfWork.Empresa.Get(u => u.Id == id);
                return View(empresa);
            }

        }

        [HttpPost]
        public IActionResult Upsert(Models.Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                if (empresa.Id == 0)
                {
                    _unitOfWork.Empresa.Add(empresa);
                }
                else
                {
                    _unitOfWork.Empresa.update(empresa);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(empresa);
        }

    }
}
