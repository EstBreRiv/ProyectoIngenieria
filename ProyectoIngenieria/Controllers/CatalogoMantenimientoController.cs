using Microsoft.AspNetCore.Mvc;
using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

public class CatalogoMantenimientoController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CatalogoMantenimientoController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Upsert(int? id)
    {
        if (id == null || id == 0)
            return View(new CatalogoMantenimiento());

        var mantenimiento = _unitOfWork.CatalogoMantenimiento.Get(x => x.Id == id);
        return View(mantenimiento);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(CatalogoMantenimiento mantenimiento)
    {
        if (ModelState.IsValid)
        {
            if (mantenimiento.Id == 0)
                _unitOfWork.CatalogoMantenimiento.Add(mantenimiento);
            else
                _unitOfWork.CatalogoMantenimiento.Update(mantenimiento);

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        return View(mantenimiento);
    }

    #region API
    [HttpGet]
    public IActionResult GetAll()
    {
        var data = _unitOfWork.CatalogoMantenimiento.GetAll();
        return Json(new { data });
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var obj = _unitOfWork.CatalogoMantenimiento.Get(u => u.Id == id);
        if (obj == null)
            return Json(new { success = false, message = "Error al eliminar" });

        _unitOfWork.CatalogoMantenimiento.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Eliminado correctamente" });
    }
    #endregion
}
