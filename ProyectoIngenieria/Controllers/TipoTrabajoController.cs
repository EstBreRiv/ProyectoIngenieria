using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Controllers
{
    public class TipoTrabajoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TipoTrabajoController(IUnitOfWork unitOfWork)
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
            var TiposTrabajo = _unitOfWork.TipoTrabajo.GetAll();
            return Json(new { data = TiposTrabajo });
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            TipoTrabajoVM tipoTrabajoVM = new()
            {
                tipoTrabajo = new Models.TipoTrabajo(),
                TiposTrabajoList = _unitOfWork.TipoTrabajo.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }).ToList()
            };

            if (id == null || id == 0)
            {
                // Create
                return View(tipoTrabajoVM);
            }
            else
            {
                // Update
                tipoTrabajoVM.tipoTrabajo = _unitOfWork.TipoTrabajo.Get(u => u.Id == id);
                if (tipoTrabajoVM.tipoTrabajo == null)
                {
                    return NotFound();
                }
                return View(tipoTrabajoVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(TipoTrabajoVM tipoTrabajoVM)
        {
            if (ModelState.IsValid)
            {
                if (tipoTrabajoVM.tipoTrabajo.Id == 0)
                {
                    _unitOfWork.TipoTrabajo.Add(tipoTrabajoVM.tipoTrabajo);
                }
                else
                {
                    _unitOfWork.TipoTrabajo.Update(tipoTrabajoVM.tipoTrabajo);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(tipoTrabajoVM);
        }
    }
}
