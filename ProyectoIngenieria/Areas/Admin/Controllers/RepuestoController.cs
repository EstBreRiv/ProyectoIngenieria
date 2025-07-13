using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ProyectoIngenieria.Utilities.RolesUsuario.Role_Admin)]

    public class RepuestoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RepuestoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var lista = _unitOfWork.Repuesto.GetAll().Where(x => x.Descripcion != "Eliminado");
            return Json(new { data = lista });
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new CatalogoRepuesto());
            }

            var repuesto = _unitOfWork.Repuesto.Get(r => r.Id == id);
            if (repuesto == null) return NotFound();

            return View(repuesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CatalogoRepuesto repuesto)
        {
            if (!ModelState.IsValid)
                return View(repuesto);

            if (repuesto.Id == 0)
            {
                _unitOfWork.Repuesto.Add(repuesto);
                TempData["success"] = "Repuesto agregado correctamente.";
            }
            else
            {
                _unitOfWork.Repuesto.Update(repuesto);
                TempData["success"] = "Repuesto actualizado correctamente.";
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        //Metodo que recibe el id del repuesto a eliminar y actualiza su descripcion con "Eliminado"
        [HttpDelete]
        public IActionResult Eliminar(int id)
        {
            var repuesto = _unitOfWork.Repuesto.Get(r => r.Id == id);
            if (repuesto == null)
            {
                TempData["error"] = "Error al eliminar el repuesto.";
                return RedirectToAction(nameof(Index));
            }

            repuesto.Descripcion = "Eliminado";
            _unitOfWork.Repuesto.Update(repuesto);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Repuesto eliminado correctamente." });

        }

        #region API
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var repuesto = _unitOfWork.Repuesto.Get(r => r.Id == id);
            if (repuesto == null)
                return Json(new { success = false, message = "Error al eliminar" });

            _unitOfWork.Repuesto.Remove(repuesto);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Repuesto eliminado correctamente" });
        }
        #endregion
    }
}