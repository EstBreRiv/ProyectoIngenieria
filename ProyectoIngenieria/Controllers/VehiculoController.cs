using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;
using System.Linq;

namespace ProyectoIngenieria.Controllers
{
    // Controlador para gestionar vehículos de las empresas, el controller permitira manejar 
    // diferentes aspeos de los vehículos como crear, editar, listar y eliminar
    public class VehiculoController : Controller
    {
        // Inyección de dependencias del unit of work
        private readonly IUnitOfWork _unitOfWork;

        // Constructor que recibe el unit of work
        public VehiculoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Acciones del controlador
        // Index: Muestra la vista principal de los vehículos
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GetAll: Devuelve todos los vehículos en formato JSON
        [HttpGet]
        public IActionResult GetAll()
        {
            // Obtiene todos los vehículos desde el repositorio y los
            // transforma en un objeto anónimo
            var vehiculos = _unitOfWork.Vehiculo.GetAll()
            
                .Select(v => new
                {
                    v.Id,
                    v.Modelo,
                    v.Estado,
                    v.Descripcion,
                    v.Placa,
                    v.Tipo,
                    EmpresaNombre = _unitOfWork.Empresa.Get(x => x.Id == v.EmpresaId).Nombre
                })
                .ToList();
            // Retorna los vehículos en formato JSON
            return Json(new { data = vehiculos });
        }

        // Upsert: Permite crear o editar un vehículo
        // recibe un id para saber si se requiere agregar un nuevo vehiculo o actualizarlo
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            // Cargar empresas para el dropdown
            var empresas = _unitOfWork.Empresa.GetAll();
            ViewBag.EmpresaList = new SelectList(empresas, "Id", "Nombre");

            // Crear una instancia del ViewModel VehiculoVM
            VehiculoVM vehiculoVM = new()
            {
                Vehiculo = new Vehiculo(),
                VehiculosList = _unitOfWork.Vehiculo.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Modelo,
                    Value = i.Id.ToString()
                }).ToList()
            };

            // Verificar si el id es nulo o 0 para determinar si se trata de una creación o edición
            if (id == null || id == 0)
            {
                // Crear nuevo - estado Activo por defecto
                vehiculoVM.Vehiculo.Estado = "Activo";
                // Retorna la vista para crear un nuevo vehículo con un modelo vacío
                return View(vehiculoVM);
            }
            else
            {
                // Editar existente, obtener el vehículo por id
                vehiculoVM.Vehiculo = _unitOfWork.Vehiculo.Get(u => u.Id == id);
                if (vehiculoVM.Vehiculo == null)
                {
                    // Si no se encuentra el vehículo, retorna NotFound
                    return NotFound();
                }
                // Retorna la vista con el vehículo encontrado
                return View(vehiculoVM);
            }
        }

        // Upsert: Maneja la creación o actualización del vehículo
        // recibe un modelo para validar los campos y guardar los datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(VehiculoVM vehiculoVM)
        {
            // Verifica si el modelo es válido
            if (ModelState.IsValid)
            {
                // Forzar estado como Activo
                vehiculoVM.Vehiculo.Estado = "Activo";

                //Si en la vista se indica que no tiene placa, se asigna null a la propiedad Placa
                var tienePlaca = Request.Form["mostrarPlaca"].Count > 0;
                if (!tienePlaca)
                {
                    vehiculoVM.Vehiculo.Placa = null;
                }

                // Si el id es 0, se trata de una creación
                if (vehiculoVM.Vehiculo.Id == 0)
                {
                    // Agrega el nuevo vehículo al repositorio
                    _unitOfWork.Vehiculo.Add(vehiculoVM.Vehiculo);
                }
                else
                {
                    // Si el id es válido, se trata de una actualización
                    _unitOfWork.Vehiculo.Update(vehiculoVM.Vehiculo);
                }
                // Guarda los cambios en el unit of work
                _unitOfWork.Save();
                // Redirige a la vista de detalle del vehículo recién creado o actualizado
                return RedirectToAction("DetalleVehiculo", new { id = vehiculoVM.Vehiculo.Id });

            }

            // Recargar empresas si hay error de validación
            var empresas = _unitOfWork.Empresa.GetAll();
            ViewBag.EmpresaList = new SelectList(empresas, "Id", "Nombre");
            return View(vehiculoVM);
        }

        // Delete: Maneja la eliminación lógica de un vehículo, recibe el id del vehiculo a eliminar
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    // si el id no es valido indica que no se encontro
                    return NotFound();
                }

                // Obtiene el vehículo por id desde el repositorio
                var vehiculo = _unitOfWork.Vehiculo.Get(u => u.Id == id);
                if (vehiculo == null)
                {
                    // Si no se encuentra el vehículo, retorna NotFound
                    return NotFound();
                }

                //soft delete
                vehiculo.Estado = "Inactivo"; // Cambiar estado a Inactivo
                //operador.VehiculoId = 0; // Desasociar operador del vehículo
                _unitOfWork.Vehiculo.Update(vehiculo);
                //_unitOfWork.Operador.Update(operador);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Se ha inactivado exitosamente" });
            }
            catch
            {
                // Si ocurre un error al eliminar, retorna un mensaje de error
                return Json(new { success = false, message = "No se pudo inactivar" });
            }
        }

        // DetalleVehiculo: Muestra los detalles de un vehículo específico, recibe el id del vehiculo
        // que se desean ver los detalles
        [HttpGet]
        public IActionResult DetalleVehiculo(int id)
        {
            // obtiene la informacion del vehiculo a ver detalles
            var vehiculo = _unitOfWork.Vehiculo.Get(v => v.Id == id, includeProperties: "Empresa");



            // Verifica si el vehículo existe y está activo
            if (vehiculo == null || vehiculo.Estado == "Inactivo")
                // Si no se encuentra el vehículo o está inactivo, retorna NotFound
                return NotFound();

            // Crea una instancia del ViewModel VehiculoVM para pasar a la vista
            var vehiculoVM = new VehiculoVM
            {
                // Asigna el vehículo encontrado al ViewModel
                Vehiculo = vehiculo
            };

            // Cargar empresas para el dropdown
            return View(vehiculoVM);
        }


        // Activar: Permite activar un vehículo, recibe el id del vehiculo a activar
        [HttpPost]
        public IActionResult Activar(int? id)
        {
            try
            {
                // Verifica si el id es nulo o 0, lo que indica que no se encontró el vehículo
                if (id == null || id == 0)
                {
                    // Si el id no es válido, retorna NotFound
                    return NotFound();
                }

                // Obtiene el vehículo por id desde el repositorio
                var vehiculo = _unitOfWork.Vehiculo.Get(u => u.Id == id);
                if (vehiculo == null)
                {
                    // Si no se encuentra el vehículo, retorna NotFound
                    return NotFound();
                }

                // Cambia el estado del vehículo a Activo
                vehiculo.Estado = "Activo";
                // Actualiza el vehículo en el repositorio
                _unitOfWork.Vehiculo.Update(vehiculo);
                // Guarda los cambios en el unit of work
                _unitOfWork.Save();

                // Retorna un mensaje de éxito en formato JSON
                return Json(new { success = true, message = "Se ha activado exitosamente" });
            }
            catch
            {
                // Si ocurre un error al activar, retorna un mensaje de error
                return Json(new { success = false, message = "No se pudo activar" });
            }
        }

    }
}