using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Controllers
{
    public class LugarTrabajoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LugarTrabajoController(IUnitOfWork unitOfWork)
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
            var LugaresTrabajo = _unitOfWork.LugarTrabajo.GetAll();
            return Json(new { data = LugaresTrabajo });
        }


        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            LugarTrabajoVM lugarTrabajoVM = new LugarTrabajoVM
            {
                lugarTrabajo = new Models.LugarTrabajo(),
                LugaresTrabajoList = _unitOfWork.LugarTrabajo.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Nombre + " - " + i.Canton,
                    Value = i.Id.ToString()
                }).ToList()
            };

            if (id == null || id == 0)
            {
                // Create
                return View(lugarTrabajoVM);
            }
            else
            {
                // Update
                lugarTrabajoVM.lugarTrabajo = _unitOfWork.LugarTrabajo.Get(u => u.Id == id);
                if (lugarTrabajoVM.lugarTrabajo == null)
                {
                    return NotFound();
                }
                return View(lugarTrabajoVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(LugarTrabajoVM lugarTrabajoVM)
        {
            if (ModelState.IsValid)
            {
                if (lugarTrabajoVM.lugarTrabajo.Id == 0)
                {
                    _unitOfWork.LugarTrabajo.Add(lugarTrabajoVM.lugarTrabajo);
                }
                else
                {
                    _unitOfWork.LugarTrabajo.Update(lugarTrabajoVM.lugarTrabajo);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(lugarTrabajoVM);
        }

    }
}

