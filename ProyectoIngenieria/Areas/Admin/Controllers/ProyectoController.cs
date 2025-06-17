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
                Proyecto = new Proyecto(),
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
    }
}
