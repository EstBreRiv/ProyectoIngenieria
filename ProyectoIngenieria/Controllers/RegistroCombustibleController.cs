using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Controllers
{
    public class RegistroCombustibleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistroCombustibleController(IUnitOfWork unitOfWork)
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
            var registrosCombustible = _unitOfWork.RegistroCombustible.GetAll()
                .Select(r => new
                {
                    r.Id,
                    r.FechaCompra,
                    r.LitrosComprados,
                    r.PrecioLitro,
                    r.TotalPagado
                })
                .ToList();
            return Json(new { data = registrosCombustible });
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            RegistroCombustibleVM registroCombustibleVM = new()
            {
                RegistroCombustible = new RegistroCombustible(),
                RegistroCombustibleList = _unitOfWork.RegistroCombustible.GetAll().Select(i => new SelectListItem
                {
                    Text = i.FechaCompra.ToString(),
                    Value = i.Id.ToString()
                }).ToList()
            };

            if (id == null || id == 0)
            {
                // Create
                return View(registroCombustibleVM);
            }
            else
            {
                // Update
                registroCombustibleVM.RegistroCombustible = _unitOfWork.RegistroCombustible.Get(u => u.Id == id);
                if (registroCombustibleVM.RegistroCombustible == null)
                {
                    return NotFound();
                }
                return View(registroCombustibleVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(RegistroCombustibleVM registroCombustibleVM)
        {
            if (ModelState.IsValid)
            {
                if (registroCombustibleVM.RegistroCombustible.Id == 0)
                {
                    _unitOfWork.RegistroCombustible.Add(registroCombustibleVM.RegistroCombustible);
                }
                else
                {
                    _unitOfWork.RegistroCombustible.Update(registroCombustibleVM.RegistroCombustible);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(registroCombustibleVM);
        }
    }
}


