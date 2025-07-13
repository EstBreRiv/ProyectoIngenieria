using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ProyectoIngenieria.Utilities.RolesUsuario.Role_Admin)]

    // Controller del lugar de trabajo en la que trabajo una maquina
    // permite crear, editar y listar los lugares de trabajo
    public class LugarTrabajoController : Controller
    {
        // Inyeccion de dependencias del unit of work
        private readonly IUnitOfWork _unitOfWork;

        // Constructor que recibe el unit of work
        public LugarTrabajoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Acciones del controlador
        // Index: Muestra la vista principal del lugar de trabajo
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GetAll: Devuelve todos los lugares de trabajo en formato JSON
        [HttpGet]
        public IActionResult GetAll()
        {
            // Obtiene todos los lugares de trabajo desde el repositorio
            var LugaresTrabajo = _unitOfWork.LugarTrabajo.GetAll().Where(x => x.Canton != "Eliminado");
            return Json(new { data = LugaresTrabajo });
        }


        // Upsert: Permite crear o editar un lugar de trabajo
        // Si el id es nulo o 0, crea un nuevo lugar de trabajo
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            // Crea una instancia del ViewModel LugarTrabajoVM
            // y llena la lista de lugares de trabajo para el dropdown
            LugarTrabajoVM lugarTrabajoVM = new LugarTrabajoVM
            {
                lugarTrabajo = new Models.LugarTrabajo(),
                LugaresTrabajoList = _unitOfWork.LugarTrabajo.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Nombre + " - " + i.Canton,
                    Value = i.Id.ToString()
                }).ToList()
            };

            // Si el id es nulo o 0, se trata de una creación
            if (id == null || id == 0)
            {
                // retorna la vista para crear con un modelo vacio
                return View(lugarTrabajoVM);
            }
            else
            {
                //Si se trata de una actualizacion, obtiene la informacion del modelo
                // desde el repositorio y la retorna a la vista
                lugarTrabajoVM.lugarTrabajo = _unitOfWork.LugarTrabajo.Get(u => u.Id == id);
                if (lugarTrabajoVM.lugarTrabajo == null)
                {
                    // Si no se encuentra el lugar de trabajo, retorna NotFound
                    return NotFound();
                }
                return View(lugarTrabajoVM);
            }
        }

        // Upsert: Maneja la creación o actualización del lugar de trabajo
        [HttpPost]
        public IActionResult Upsert(LugarTrabajoVM lugarTrabajoVM)
        {
            // Verifica si el modelo es válido
            if (ModelState.IsValid)
            {
                // Si el id es 0, se trata de una creación
                if (lugarTrabajoVM.lugarTrabajo.Id == 0)
                {
                    // Agrega el nuevo lugar de trabajo al repositorio
                    _unitOfWork.LugarTrabajo.Add(lugarTrabajoVM.lugarTrabajo);
                }
                else
                {
                    // Si el id no es 0, se trata de una actualización
                    _unitOfWork.LugarTrabajo.Update(lugarTrabajoVM.lugarTrabajo);
                }
                // Guarda los cambios en el unit of work
                _unitOfWork.Save();
                // Redirige a la vista principal del lugar de trabajo
                return RedirectToAction("Index");
            }
            return View(lugarTrabajoVM);
        }

        // Delete: Elimina un lugar de trabajo por su id
        [HttpDelete]
        public IActionResult Eliminar(int id)
        {
            // Obtiene el lugar de trabajo por su id
            var lugarTrabajo = _unitOfWork.LugarTrabajo.Get(u => u.Id == id);
            if (lugarTrabajo == null)
            {
                // Si no se encuentra, retorna NotFound
                return Json(new { success = false, message = "No se encontró el registro." });
            }


            //Actualiza el canton como eliminado
            lugarTrabajo.Canton = "Eliminado";

            // Guarda los cambios en el unit of work
            _unitOfWork.Save();
            // Retorna una respuesta exitosa
            return Json(new { success = true, message = "Lugar de trabajo eliminado exitosamente." });
        }


    }
}

