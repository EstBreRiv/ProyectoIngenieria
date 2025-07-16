using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;
using System.Linq;

namespace ProyectoIngenieria.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ProyectoIngenieria.Utilities.RolesUsuario.Role_Admin)]

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
            var empresas = _unitOfWork.Empresa.GetAll().Select(e => new SelectListItem
            {
                Text = e.Nombre,
                Value = e.Nombre
            }).ToList();

            ViewBag.Empresas = empresas;
            return View();
        }

        // GetAll: Devuelve todos los vehículos en formato JSON
        [HttpGet]
        public IActionResult GetAll()
        {
            //crea una lista de vehiculos utilizando el view model VehiculoVM
            var vehiculos = _unitOfWork.Vehiculo.GetAll(includeProperties: "Empresa,Marca,TipoVehiculo")
                .Select(v => new
                {
                    v.Id,
                    v.Modelo,
                    v.Estado,
                    v.Placa,
                    Empresa = v.Empresa.Nombre,
                    Marca = v.Marca.NombreMarca,
                    TipoVehiculo = v.TipoVehiculo.Tipo
                }).ToList();

            // Retorna los vehículos en formato JSON
            return Json(new { data = vehiculos });
        }

        // Upsert: Permite crear o editar un vehículo
        // recibe un id para saber si se requiere agregar un nuevo vehiculo o actualizarlo
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            // Cargar empresas para el dropdown
            var empresas = _unitOfWork.Empresa.GetAll().Where(e => e.Nombre != "Eliminado");
            ViewBag.EmpresaList = new SelectList(empresas, "Id", "Nombre");

            // Cargar tipos de vehículos para el dropdown
            var tiposVehiculo = _unitOfWork.TipoVehiculo.GetAll().Where(x => x.Descripcion != "Eliminado");
            ViewBag.TipoVehiculoList = new SelectList(tiposVehiculo, "Id", "Tipo");

            //Faltan cargas las marcas y los tipos de vehiculos
            var marcas = _unitOfWork.Marca.GetAll().Where(m => m.NombreMarca != "Eliminado");
            ViewBag.MarcaList = new SelectList(marcas, "Id", "NombreMarca");

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
            var vehiculo = _unitOfWork.Vehiculo.Get(v => v.Id == id, includeProperties: "Empresa,Marca,TipoVehiculo");

            // Crea una instancia del ViewModel VehiculoVM para pasar a la vista
            var vehiculoVM = new VehiculoVM
            {
                // Asigna el vehículo encontrado al ViewModel
                Vehiculo = vehiculo
            };

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

        //-----------------------------------------------------------------------------

        // Acciones para manejar los documentos de los vehiculos
        //permite enviar a la el modelo con el vehiculo al asignar documentos
        [HttpGet]
        public IActionResult CargarDocumentoVehiculo(int id)
        {
            DocumentoVehiculoVM documentoVehiculoVM = new()
            {
                DocumentoVehiculo = new DocumentoVehiculo()
                {
                    VehiculoId = id
                }
            };

            return View(documentoVehiculoVM);
        }

        // Carga un documento para un vehiculo específico
        [HttpPost]
        public IActionResult CargarDocumentoVehiculo(DocumentoVehiculoVM documentoVehiculoVM)
        {
            // Verifica si el modelo es válido
            if (ModelState.IsValid)
            {
                // Verifica si se ha subido un archivo
                if (documentoVehiculoVM.Archivo != null && documentoVehiculoVM.Archivo.Length > 0)
                {
                    // Genera un nombre único para el archivo
                    var nombreArchivo = Path.GetFileNameWithoutExtension(documentoVehiculoVM.Archivo.FileName);
                    var extension = Path.GetExtension(documentoVehiculoVM.Archivo.FileName);
                    var nombreUnico = $"{nombreArchivo}_{DateTime.Now.Ticks}{extension}";
                    var rutaGuardar = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documentos", "documentoVehiculo", nombreUnico);

                    // Asegura que el directorio exista
                    using (var stream = new FileStream(rutaGuardar, FileMode.Create))
                    {
                        // Copia el archivo subido al directorio especificado
                        documentoVehiculoVM.Archivo.CopyTo(stream);
                    }

                    // Guardar el path del archivo en la base de datos
                    documentoVehiculoVM.DocumentoVehiculo.Ruta = "/documentos/documentoVehiculo/" + nombreUnico;
                }

                // Guardar el registro en base de datos
                _unitOfWork.DocumentoVehiculo.Add(documentoVehiculoVM.DocumentoVehiculo);
                _unitOfWork.Save();

                return RedirectToAction("DocumentoVehiculo", new { id = documentoVehiculoVM.DocumentoVehiculo.VehiculoId });
            }

            return View(documentoVehiculoVM);
        }

        // Muestra los documentos de un vehiculo específico
        [HttpGet]
        public IActionResult DocumentoVehiculo(int id)
        {
            // Verifica si el vehiculo existe
            var vehiculo = _unitOfWork.Vehiculo.Get(u => u.Id == id);

            if (vehiculo == null)
            {
                // Si no se encuentra el vehiculo, retorna NotFound
                return NotFound();
            }

            // Crea una instancia del ViewModel VehiculoVM con el operador encontrado
            VehiculoVM vehiculoVM = new VehiculoVM
            {
                Vehiculo = vehiculo
            };

            // Obtiene la informacion del vehiculo y los asigna al ViewModel
            return View(vehiculoVM);
        }

        // Obtiene los documentos de un vehiculo específico
        //obtiene un id para identificar el vehiculo
        [HttpGet]
        public IActionResult GetDocumentosVehiculo(int id)
        {
            // Verifica si el vehiculo existe
            //obtiene los documentos del vehiculo por su cédula
            var documentos = _unitOfWork.DocumentoVehiculo
                .GetAll()
                .Where(d => d.VehiculoId == id)
                .Select(d => new
                {
                    d.Id,
                    d.Nombre,
                    d.Ruta
                })
                .ToList();

            // retorna los documentos en formato JSON
            return Json(new { data = documentos });
        }

        // Elimina un documento de un vehiculo específico
        [HttpDelete]
        public IActionResult DeleteDocumento(int id)
        {
            // Busca el documento por su ID
            var documento = _unitOfWork.DocumentoVehiculo.Get(d => d.Id == id);
            if (documento == null)
            {
                // Si no se encuentra el documento, retorna un mensaje de error
                return Json(new { success = false, message = "Error al borrar el documento" });
            }

            // Elimina el archivo físico del servidor
            var rutaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", documento.Ruta.TrimStart('/'));
            if (System.IO.File.Exists(rutaFisica))
            {
                System.IO.File.Delete(rutaFisica);
            }

            // Elimina la ruta del documento de la base de datos y guarda los cambios
            _unitOfWork.DocumentoVehiculo.Remove(documento);
            _unitOfWork.Save();

            // Retorna un mensaje de éxito al eliminar el documento
            return Json(new { success = true, message = "Documento eliminado exitosamente" });
        }

    }
}