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

    //Controller de las empresas que permiten identificar la pertenecia de las maquinas
    //El controller permite crear, editar y listar las empresas
    public class EmpresaController : Controller
    {
        //Inyeccion de dependencias del UnitOfWork para acceder a los repositorios
        private readonly IUnitOfWork _unitOfWork;
        //Constructor que recibe el UnitOfWork
        public EmpresaController(IUnitOfWork unitOfWork)
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
            var empresas = _unitOfWork.Empresa.GetAll().Where(e => e.Nombre != "Eliminado");
            return Json(new { data = empresas });
        }

        //Upsert: Permite crear o editar una empresa
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            //Crea un modelo de vista EmpresaVM
            //que contiene la empresa y una lista de empresas para el dropdown
            EmpresaVM empresaVM = new()
            {
                Empresa = new Empresa(),
                EmpresasList = _unitOfWork.Empresa.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }).ToList()
            };

            if (id == null || id == 0)
            {
                // En el caso de que el id sea nulo o cero, se trata de una creación
                return View(empresaVM);
            }
            else
            {
                // En el caso de que el id sea válido, se trata de una edición
                empresaVM.Empresa = _unitOfWork.Empresa.Get(u => u.Id == id);
                if (empresaVM.Empresa == null)
                {
                    // Si no se encuentra la empresa, se devuelve NotFound
                    return NotFound();
                }
                // Se actualiza la lista de empresas para el dropdown
                return View(empresaVM);
            }
        }


        //Upsert: Permite crear o editar una empresa, recibe el modelo de vista EmpresaVM
        [HttpPost]
        public IActionResult Upsert(EmpresaVM empresaVM)
        {
            //Valida el modelo recibido, si no es válido, devuelve la vista con los errores
            if (ModelState.IsValid)
            {
                if (empresaVM.Empresa.Id == 0)
                {
                    // Si el Id es 0, se trata de una creación de una nueva empresa
                    _unitOfWork.Empresa.Add(empresaVM.Empresa);
                }
                else
                {
                    // Si el Id es diferente de 0, se trata de una edición de una empresa existente
                    _unitOfWork.Empresa.update(empresaVM.Empresa);
                }
                // Guarda los cambios en la base de datos
                _unitOfWork.Save();
                // Redirige a la vista principal de empresas
                return RedirectToAction("Index");
            }
            return View(empresaVM);
        }

        [HttpDelete]
        public IActionResult Eliminar(int id)
        {
            var empresa = _unitOfWork.Empresa.Get(u => u.Id == id);
            if (empresa == null)
                return NotFound();

            // Actualiza la descripción a "Eliminado" en lugar de eliminarlo físicamente
            empresa.Nombre = "Eliminado";
            _unitOfWork.Empresa.update(empresa);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Empresa eliminada correctamente." });

        }

    }
}
