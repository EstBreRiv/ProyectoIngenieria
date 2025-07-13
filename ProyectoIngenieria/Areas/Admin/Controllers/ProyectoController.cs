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

    // Controlador para gestionar proyectos relacionados con los trabajos de las maquinas
    // Permite crear, editar y listar los proyectos
    public class ProyectoController : Controller
    {
        // Inyección de dependencias del unit of work
        private readonly IUnitOfWork _unitOfWork;

        // Constructor que recibe el unit of work
        public ProyectoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Acciones del controlador
        // Index: Muestra la vista principal de los proyectos
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GetAll: Devuelve todos los proyectos en formato JSON
        [HttpGet]
        public IActionResult GetAll()
        {
            // Obtiene todos los proyectos desde el repositorio y los retiorna como JSON
            var proyectos = _unitOfWork.Proyecto.GetAll().Where(x=>x.Cliente != "Eliminado");
            return Json(new { data = proyectos });
        }

        // Upsert: Permite crear o editar un proyecto
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            // Crea una instancia del ViewModel ProyectoVM
            ProyectoVM proyectoVM = new ProyectoVM
            {
                Proyecto = new Proyecto(),
                ProyectoList = _unitOfWork.Proyecto.GetAll().Select(i => new SelectListItem
                {
                    Text = i.NombreProyecto,
                    Value = i.Id.ToString()
                }).ToList()
            };

            // Si el id es nulo o 0, se trata de una creación
            if (id == null || id == 0)
            {
                proyectoVM.Proyecto.FechaInicio = DateOnly.FromDateTime(DateTime.Today);
                proyectoVM.Proyecto.FechaFin = DateOnly.FromDateTime(DateTime.Today);
                return View(proyectoVM);
            }

            else
            {
                // Si el id es válido, se trata de una edición
                // Obtiene el proyecto por id desde el repositorio 
                proyectoVM.Proyecto = _unitOfWork.Proyecto.Get(u => u.Id == id);
                if (proyectoVM.Proyecto == null)
                {
                    // Si no se encuentra el proyecto, retorna NotFound
                    return NotFound();
                }
                // Retorna la vista con el proyecto encontrado
                return View(proyectoVM);
            }

        }

        //Upsert: Permite crear o editar una empresa, recibe el modelo de vista EmpresaVM
        [HttpPost]
        public IActionResult Upsert(ProyectoVM proyectoVM)
        {

            if (proyectoVM.Proyecto.FechaInicio > proyectoVM.Proyecto.FechaFin)
                ModelState.AddModelError("Proyecto.FechaInicio", "La fecha de inicio del proyecto no puede ser mayor a la fecha de cierre.");
            else if (proyectoVM.Proyecto.FechaFin < proyectoVM.Proyecto.FechaInicio)
                ModelState.AddModelError("Proyecto.FechaFin", "La fecha de cierre del proyecto no puede ser menor a la fecha de inicio.");

            //Valida el modelo recibido, si no es válido, devuelve la vista con los errores
            if (ModelState.IsValid)
            {
                if (proyectoVM.Proyecto.Id == 0)
                {
                    // Si el Id es 0, se trata de una creación de una nueva empresa
                    _unitOfWork.Proyecto.Add(proyectoVM.Proyecto);
                }
                else
                {
                    // Si el Id es diferente de 0, se trata de una edición de una empresa existente
                    _unitOfWork.Proyecto.Update(proyectoVM.Proyecto);
                }
                // Guarda los cambios en la base de datos
                _unitOfWork.Save();
                // Redirige a la vista principal de empresas
                return RedirectToAction("Index");
            }
            return View(proyectoVM);
        }


        [HttpDelete]
        public IActionResult Eliminar(int id)
        {
            var proyectos = _unitOfWork.Proyecto.Get(h => h.Id == id);
            if (proyectos == null)
            {
                return Json(new { success = false, message = "No se encontró el proyecto." });
            }

            //Actualiza el nombre del cliente a Eliminado
            proyectos.Cliente = "Eliminado";

            _unitOfWork.Proyecto.Update(proyectos);

            _unitOfWork.Save();
            return Json(new { success = true, message = "Proyecto eliminado exitosamente." });
        }
    }
}
