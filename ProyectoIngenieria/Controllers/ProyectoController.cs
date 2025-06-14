using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Controllers
{
    public class ProyectoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProyectoController(IUnitOfWork unitOfWork)
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
            var proyectos = _unitOfWork.Proyecto.GetAll();
            return Json(new { data = proyectos });
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ProyectoVM proyectoVM = new ProyectoVM
            {
                Proyecto = new Models.Proyecto(),
                ProyectoList = _unitOfWork.Proyecto.GetAll().Select(i => new SelectListItem
                {
                    Text = i.NombreProyecto,
                    Value = i.Id.ToString()
                }).ToList()
            };

            if (id == null || id == 0)
            {
                // Create
                return View(proyectoVM);
            }
            else
            {
                // Update
                proyectoVM.Proyecto = _unitOfWork.Proyecto.Get(u => u.Id == id);
                if (proyectoVM.Proyecto == null)
                {
                    return NotFound();
                }
                return View(proyectoVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProyectoVM proyectoVM)
        {
            if (ModelState.IsValid)
            {
                if (proyectoVM.Proyecto.Id == 0)
                {
                    _unitOfWork.Proyecto.Add(proyectoVM.Proyecto);
                }
                else
                {
                    _unitOfWork.Proyecto.Update(proyectoVM.Proyecto);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(proyectoVM);
        }
    }
}
