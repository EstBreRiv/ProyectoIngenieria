using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ProyectoIngenieria.Utilities.RolesUsuario.Role_Admin)]

    // Controlador para gestionar los tipos de trabajo relacionados
    // a las horas de trabajo de las maquinas
    // Permite crear, editar y listar los tipos de trabajo
    public class TipoTrabajoController : Controller
    {
        // Inyección de dependencias del unit of work
        private readonly IUnitOfWork _unitOfWork;

        // Constructor que recibe el unit of work
        public TipoTrabajoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Acciones del controlador
        // Index: Muestra la vista principal de los tipos de trabajo
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GetAll: Devuelve todos los tipos de trabajo en formato JSON
        [HttpGet]
        public IActionResult GetAll()
        {
            // Obtiene todos los tipos de trabajo desde el repositorio
            var TiposTrabajo = _unitOfWork.TipoTrabajo.GetAll().Where(x => x.Descripcion != "Eliminado");
            // Retorna los tipos de trabajo en formato JSON
            return Json(new { data = TiposTrabajo });
        }

        // Upsert: Permite crear o editar un tipo de trabajo recibe el id del tipo de trabajo
        // Si el id es nulo o 0, crea un nuevo tipo de trabajo
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            // Crea una instancia del ViewModel TipoTrabajoVM
            // agrega los datos de los tipos de trabajo
            TipoTrabajoVM tipoTrabajoVM = new()
            {
                tipoTrabajo = new Models.TipoTrabajo(),
                TiposTrabajoList = _unitOfWork.TipoTrabajo.GetAll().Where(x => x.Descripcion != "Eliminado").Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }).ToList()
            };

            if (id == null || id == 0)
            {
                // Si el id enviado por parametros no existe se trata de una creación
                // Retorna la vista para crear un nuevo tipo de trabajo con un modelo vacío
                return View(tipoTrabajoVM);
            }
            else
            {
                //Si el id es válido, se trata de una edición
                // Obtiene el tipo de trabajo por id desde el repositorio
                tipoTrabajoVM.tipoTrabajo = _unitOfWork.TipoTrabajo.Get(u => u.Id == id);
                if (tipoTrabajoVM.tipoTrabajo == null)
                {
                    // Si no se encuentra el tipo de trabajo, retorna NotFound
                    return NotFound();
                }
                // Retorna la vista con el tipo de trabajo encontrado
                return View(tipoTrabajoVM);
            }
        }

        // Upsert: Maneja la creación o actualización del tipo de trabajo
        //recibe un modelo para validar los campos y guardar los datos
        [HttpPost]
        public IActionResult Upsert(TipoTrabajoVM tipoTrabajoVM)
        {
            // Verifica si el modelo es válido
            if (ModelState.IsValid)
            {
                // Si el id es 0, se trata de una creación
                if (tipoTrabajoVM.tipoTrabajo.Id == 0)
                {
                    // lo agrega a la lista de tipos de trabajo
                    _unitOfWork.TipoTrabajo.Add(tipoTrabajoVM.tipoTrabajo);
                }
                else
                {
                    // Si el id es válido, se trata de una actualización
                    // Actualiza el tipo de trabajo existente
                    _unitOfWork.TipoTrabajo.Update(tipoTrabajoVM.tipoTrabajo);
                }
                // Guarda los cambios en el repositorio
                _unitOfWork.Save();
                // Redirige a la vista principal de tipos de trabajo
                return RedirectToAction("Index");
            }
            // Si el modelo no es válido, retorna la vista con el modelo actual
            return View(tipoTrabajoVM);
        }


        // Delete: Elimina un tipo de trabajo por su id
        [HttpDelete]
        public IActionResult Eliminar(int id)
        {
            // Obtiene el tipo de trabajo por id desde el repositorio
            var tipoTrabajo = _unitOfWork.TipoTrabajo.Get(u => u.Id == id);
            if (tipoTrabajo == null)
            {
                // Si no se encuentra el tipo de trabajo, retorna NotFound
                return NotFound();
            }

            //Actualiza su descripcion como eliminado
            tipoTrabajo.Descripcion = "Eliminado";
            // Actualiza el tipo de trabajo en el repositorio
            _unitOfWork.TipoTrabajo.Update(tipoTrabajo);
            // Guarda los cambios en el repositorio
            _unitOfWork.Save();
            // Retorna una respuesta exitosa
            return Json(new { success = true, message = "Tipo de trabajo eliminado correctamente." });
        }
    }
}
