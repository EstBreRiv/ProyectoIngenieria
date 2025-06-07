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
        public IActionResult Index(int id)
        {
            ViewBag.VehiculoId = id; // Pasa el ID a la vista para que el JS lo use
            return View();
        }

        [HttpGet]
        public IActionResult GetAll(int id)
        {
            var registrosCombustible = _unitOfWork.RegistroCombustible.GetAll(r => r.VehiculoId == id)
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
        public IActionResult Upsert(int? id, int? vehiculoId) //Puede recibir 2 id
        {
            RegistroCombustibleVM registroCombustibleVM = new()
            {
                RegistroCombustible = new RegistroCombustible()
                //RegistroCombustibleList = _unitOfWork.RegistroCombustible.GetAll().Select(i => new SelectListItem
                //{
                //    Text = i.FechaCompra.ToString(),
                //    Value = i.Id.ToString()
                //}).ToList()
            };

            if (id == null || id == 0)
            {
                // Create
                if (vehiculoId.HasValue)
                {
                    registroCombustibleVM.RegistroCombustible.VehiculoId = vehiculoId.Value;
                    ViewBag.VehiculoId = vehiculoId.Value; //Agarra el vehiculoId desde la URL y lo pasa al viewbag y al VM, luego se manda a la vista
                }
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

                ViewBag.VehiculoId = registroCombustibleVM.RegistroCombustible.VehiculoId; //Util para edicion

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
                return RedirectToAction("Index", new { id = registroCombustibleVM.RegistroCombustible.VehiculoId }); //Para que se redirija con la URL con id del vehiculo asociado
            }
            return View(registroCombustibleVM);
        }
    }
}


