using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;
using ProyectoIngenieria.Models;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoIngenieria.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ProyectoIngenieria.Utilities.RolesUsuario.Role_Admin)]

    public class MarcaController : Controller
    {
        //Inyeccion de dependencias del UnitOfWork para acceder a los repositorios
        private readonly IUnitOfWork _unitOfWork;
        //Constructor que recibe el UnitOfWork
        public MarcaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Acciones del controller
        //Index: Muestra la vista principal de las empresas
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //GetAll: Devuelve todas las empresas en formato JSON para ser utilizadas en DataTables
        [HttpGet]
        public IActionResult GetAll()
        {
            //Obtiene todas las empresas desde el repositorio y las devuelve en formato JSON
            var marcas = _unitOfWork.Marca.GetAll().Where(m => m.NombreMarca != "Eliminado");
            return Json(new { data = marcas });
        }

        //Upsert: Permite crear o editar una empresa
        [HttpGet]
        public IActionResult Upsert(int? id)
        {


            MarcaVM marcaVM = new()
            {
                marca = new Marca(),
                MarcasList = _unitOfWork.Marca.GetAll().Select(i => new SelectListItem
                {
                    Text = i.NombreMarca,
                    Value = i.Id.ToString()
                }).ToList()
            };

            if (id == null || id == 0)
            {
                // En el caso de que el id sea nulo o cero, se trata de una creación
                return View(marcaVM);
            }
            else
            {
                // En el caso de que el id sea válido, se trata de una edición
                marcaVM.marca = _unitOfWork.Marca.Get(u => u.Id == id);
                if (marcaVM.marca == null)
                {
                    // Si no se encuentra la empresa, se devuelve NotFound
                    return NotFound();
                }
                // Se actualiza la lista de empresas para el dropdown
                return View(marcaVM);
            }
        }


        //Upsert: Permite crear o editar una empresa, recibe el modelo de vista EmpresaVM
        [HttpPost]
        public IActionResult Upsert(MarcaVM marcaVM)
        {
            //Valida el modelo recibido, si no es válido, devuelve la vista con los errores
            if (ModelState.IsValid)
            {
                if (marcaVM.marca.Id == 0)
                {
                    // Si el Id es 0, se trata de una creación de una nueva empresa
                    _unitOfWork.Marca.Add(marcaVM.marca);
                }
                else
                {
                    // Si el Id es diferente de 0, se trata de una edición de una empresa existente
                    _unitOfWork.Marca.Update(marcaVM.marca);
                }
                // Guarda los cambios en la base de datos
                _unitOfWork.Save();
                // Redirige a la vista principal de empresas
                return RedirectToAction("Index");
            }
            return View(marcaVM);
        }

        [HttpDelete]
        public IActionResult Eliminar(int id)
        {
            var marca = _unitOfWork.Marca.Get(u => u.Id == id);
            if (marca == null)
                return NotFound();

            // Actualiza la descripción a "Eliminado" en lugar de eliminarlo físicamente
            marca.NombreMarca = "Eliminado";
            _unitOfWork.Marca.Update(marca);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Marca eliminada correctamente." });

        }
    }
}
