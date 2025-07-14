using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ProyectoIngenieria.Utilities.RolesUsuario.Role_Admin)]

    public class TipoVehiculoController : Controller
    {
        // Inyección de dependencias del unit of work
        private readonly IUnitOfWork _unitOfWork;

        // Constructor que recibe el unit of work
        public TipoVehiculoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        //GetAll: Devuelve todas las empresas en formato JSON para ser utilizadas en DataTables
        [HttpGet]
        public IActionResult GetAll()
        {
            //Obtiene todas las empresas desde el repositorio y las devuelve en formato JSON
            var tiposVehiculos = _unitOfWork.TipoVehiculo.GetAll().Where(x=>x.Descripcion!="Eliminado");
            return Json(new { data = tiposVehiculos });
        }

        //Upsert: Permite crear o editar una empresa
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            //Crea un modelo de vista EmpresaVM
            //que contiene la empresa y una lista de empresas para el dropdown
            TipoVehiculoVM tipoVehiculoVM = new()
            {
                tipoVehiculo = new Models.TipoVehiculo(),
                TiposVehiculoList = _unitOfWork.TipoVehiculo.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Tipo,
                    Value = i.Id.ToString()
                }).ToList()
            };

            if (id == null || id == 0)
            {
                // En el caso de que el id sea nulo o cero, se trata de una creación
                return View(tipoVehiculoVM);
            }
            else
            {
                // En el caso de que el id sea válido, se trata de una edición
                tipoVehiculoVM.tipoVehiculo = _unitOfWork.TipoVehiculo.Get(u => u.Id == id);
                if (tipoVehiculoVM.tipoVehiculo == null)
                {
                    // Si no se encuentra la empresa, se devuelve NotFound
                    return NotFound();
                }
                // Se actualiza la lista de empresas para el dropdown
                return View(tipoVehiculoVM);
            }
        }


        //Upsert: Permite crear o editar una empresa, recibe el modelo de vista EmpresaVM
        [HttpPost]
        public IActionResult Upsert(TipoVehiculoVM tipoVehiculoVM)
        {
            //Valida el modelo recibido, si no es válido, devuelve la vista con los errores
            if (ModelState.IsValid)
            {
                if (tipoVehiculoVM.tipoVehiculo.Id == 0)
                {
                    // Si el Id es 0, se trata de una creación de una nueva empresa
                    _unitOfWork.TipoVehiculo.Add(tipoVehiculoVM.tipoVehiculo);
                }
                else
                {
                    // Si el Id es diferente de 0, se trata de una edición de una empresa existente
                    _unitOfWork.TipoVehiculo.Update(tipoVehiculoVM.tipoVehiculo);
                }
                // Guarda los cambios en la base de datos
                _unitOfWork.Save();
                // Redirige a la vista principal de empresas
                return RedirectToAction("Index");
            }
            return View(tipoVehiculoVM);
        }

        //Delete: Elimina una empresa por su id
        [HttpDelete]
        public IActionResult Eliminar(int id)
        {
            // Obtiene la empresa por id desde el repositorio
            var tipoVehiculo = _unitOfWork.TipoVehiculo.Get(u => u.Id == id);
            if (tipoVehiculo == null)
            {
                // Si no se encuentra la empresa, retorna NotFound
                return Json(new { success = false, message = "No se encontró el tipo de vehiculo." });
            }
            //actualiza la descripcion como eliminado
            tipoVehiculo.Descripcion = "Eliminado";

            // Actualiza la empresa en el repositorio
            _unitOfWork.TipoVehiculo.Update(tipoVehiculo);

            // Guarda los cambios en el unit of work
            _unitOfWork.Save();
            // Retorna una respuesta exitosa
            return Json(new { success = true, message = "Se elimino el tipo de vehiculo correctamente." });
        }
    }
}
