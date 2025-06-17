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
            EmpresaVM empresaVM = new()
            {
                Empresa = new Empresa(),
                EmpresasList = _unitOfWork.Empresa.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }).ToList()
            };

            if (id == null || id == 0)
            {
                // Create
                return View(empresaVM);
            }
            else
            {
                // Update
                empresaVM.Empresa = _unitOfWork.Empresa.Get(u => u.Id == id);
                if (empresaVM.Empresa == null)
                {
                    return NotFound();
                }
                return View(empresaVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(EmpresaVM empresaVM)
        {
            if (ModelState.IsValid)
            {
                if (empresaVM.Empresa.Id == 0)
                {
                    _unitOfWork.Empresa.Add(empresaVM.Empresa);
                }
                else
                {
                    _unitOfWork.Empresa.update(empresaVM.Empresa);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(empresaVM);
        }

    }
}
