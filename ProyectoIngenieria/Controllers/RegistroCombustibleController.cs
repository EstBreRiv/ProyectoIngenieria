using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository;
using ProyectoIngenieria.Repository.Interfaces;
using System.Globalization;

namespace ProyectoIngenieria.Controllers
{
    // Controlador para gestionar los registros de combustible de los vehículos y maquinaria
    // Permite crear, editar, listar y eliminar registros de combustible
    public class RegistroCombustibleController : Controller
    {
        // Inyección de dependencias del unit of work
        private readonly IUnitOfWork _unitOfWork;

        // Constructor que recibe el unit of work
        public RegistroCombustibleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Acciones del controlador
        // Index: Muestra la vista principal de los registros de combustible segun el id del vehículo
        [HttpGet]
        public IActionResult Index(int id)
        {
            ViewBag.VehiculoId = id; // Pasa el ID a la vista para que el JS lo use
            return View();
        }

        // GetAll: Devuelve todos los registros de
        // combustible para un vehículo específico en formato JSON
        [HttpGet]
        public IActionResult GetAll(int? id, DateOnly? fechaInicio, DateOnly? fechaFin)
        {
            var registros = _unitOfWork.RegistroCombustible.GetAll(
                    r =>
                    (!id.HasValue || id == 0 || r.VehiculoId == id) &&
                    (!fechaInicio.HasValue || r.FechaCompra >= fechaInicio.Value) &&
                    (!fechaFin.HasValue || r.FechaCompra <= fechaFin.Value)
            );

            var data = registros.Select(r =>
            {
                var vehiculo = _unitOfWork.Vehiculo.Get(v => v.Id == r.VehiculoId);

                return new
                {
                    id = r.Id,
                    fechaCompra = r.FechaCompra.ToString("yyyy-MM-dd"),
                    litrosComprados = r.LitrosComprados,
                    precioLitro = r.PrecioLitro,
                    totalPagado = r.TotalPagado,
                    nombreVehiculo = vehiculo != null ? $"{vehiculo.Modelo} - {vehiculo.Placa}" : ""
                };
            }).ToList();

            return Json(new { data });
        }






        // Upsert: Permite crear o editar un registro de combustible
        // tiene un id opcional para editar o crear un nuevo registro y un id de vehiculo
        //para saber a que vehiculo se le esta haciendo el registro
        [HttpGet]
        public IActionResult Upsert(int? id, int? vehiculoId) //Puede recibir 2 id
        {
            // Verifica la cultura actual para formatear correctamente las fechas y números
            string culturaActual = CultureInfo.CurrentCulture.Name;
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
                // permite verificar si el id es nulo o 0, lo que indica que se trata de una creación
                if (vehiculoId.HasValue)
                {
                    // Si se proporciona un vehiculoId, se asigna al registro de combustible
                    registroCombustibleVM.RegistroCombustible.VehiculoId = vehiculoId.Value;
                    ViewBag.VehiculoId = vehiculoId.Value; //Agarra el vehiculoId desde la URL y lo pasa al viewbag y al VM, luego se manda a la vista
                }
                // se retorna la vista para crear un nuevo registro de combustible con el modelo
                return View(registroCombustibleVM);
            }
            else
            {
                //si se requiere actualizar un registro existente, se obtiene el registro por id
                registroCombustibleVM.RegistroCombustible = _unitOfWork.RegistroCombustible.Get(u => u.Id == id);
                if (registroCombustibleVM.RegistroCombustible == null)
                {
                    // Si no se encuentra el registro, retorna NotFound
                    return NotFound();
                }

                // Si se encuentra el registro, se asigna el VehiculoId al ViewBag para que se use en la vista
                ViewBag.VehiculoId = registroCombustibleVM.RegistroCombustible.VehiculoId; //Util para edicion

                // Retorna la vista con el registro de combustible encontrado
                return View(registroCombustibleVM);
            }
        }

        // Upsert: Maneja la creación o actualización del registro de combustible
        // recibe un modelo para validar los campos y guardar los datos
        [HttpPost]
        public IActionResult Upsert(RegistroCombustibleVM registroCombustibleVM)
        {
            // Validación manual adicional por si los campos son nullable
            if (registroCombustibleVM.RegistroCombustible.LitrosComprados == null)
            {
                ModelState.AddModelError("RegistroCombustible.LitrosComprados", "Este campo es obligatorio.");
            }

            if (registroCombustibleVM.RegistroCombustible.PrecioLitro == null)
            {
                ModelState.AddModelError("RegistroCombustible.PrecioLitro", "Este campo es obligatorio.");
            }

            if (registroCombustibleVM.RegistroCombustible.FechaCompra == null)
            {
                ModelState.AddModelError("RegistroCombustible.FechaCompra", "Debe ingresar la fecha de compra.");
            }

            // Verifica si el modelo es válido
            if (ModelState.IsValid)
            {
                // Si el id es 0, se trata de una creación
                if (registroCombustibleVM.RegistroCombustible.Id == 0)
                {
                    // Agrega el nuevo registro de combustible al repositorio
                    _unitOfWork.RegistroCombustible.Add(registroCombustibleVM.RegistroCombustible);
                }
                else
                {
                    // Si el id no es 0, se trata de una actualización
                    _unitOfWork.RegistroCombustible.Update(registroCombustibleVM.RegistroCombustible);
                }
                // Guarda los cambios en el unit of work
                _unitOfWork.Save();

                //Para que se redirija con la URL con id del vehiculo asociado
                return RedirectToAction("Index", new { id = registroCombustibleVM.RegistroCombustible.VehiculoId }); 
            }

            // Si el modelo no es válido, asegurarse de mantener el VehiculoId en ViewBag
            ViewBag.VehiculoId = registroCombustibleVM.RegistroCombustible.VehiculoId;

            return View(registroCombustibleVM);
        }

        // Delete: Elimina un registro de combustible por su ID
        //recibe el id del registro a eliminar
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            // Verifica si el registro existe en la base de datos
            var registro = _unitOfWork.RegistroCombustible.Get(r => r.Id == id);
            if (registro == null)
            {
                // Si no se encuentra el registro, retorna un mensaje de error
                return Json(new { success = false, message = "Error al eliminar el registro." });
            }

            // Si el registro existe, lo elimina del repositorio y guarda los cambios
            _unitOfWork.RegistroCombustible.Remove(registro);
            _unitOfWork.Save();
            // Retorna un mensaje de éxito en formato JSON
            return Json(new { success = true, message = "Registro eliminado exitosamente." });
        }
    }
}


