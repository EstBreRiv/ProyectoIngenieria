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

    // Controlador para las personas que operan las máquinas
    // Permite crear, editar, listar y eliminar operadores
    public class OperadorController : Controller
    {
        // Inyección de dependencias del unit of work
        private readonly IUnitOfWork _unitOfWork;

        // Constructor que recibe el unit of work
        public OperadorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Acciones del controlador
        // Index: Muestra la vista principal de los operadores
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll(string tipo)
        {
            var operadores = _unitOfWork.Operador.GetAll().ToList();

            if (string.IsNullOrEmpty(tipo))
            {
                operadores = operadores.Where(o => o.TipoColaborador != "Inactivo").ToList();
            }
            else if (tipo == "Interno" || tipo == "Externo")
            {
                operadores = operadores.Where(o => o.TipoColaborador == tipo).ToList();
            }
            else if (tipo == "Inactivo")
            {
                operadores = operadores.Where(o => o.TipoColaborador == "Inactivo").ToList();
            }

            return Json(new { data = operadores });
        }


        // Upsert: Permite crear o editar un operador
        //Recibe un id opcional para determinar si es una creación o edición
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            // Crea una instancia del ViewModel OperadorVM, donde los vehiculos no sean inactivos
            var vehiculos = _unitOfWork.Vehiculo.GetAll()
                .Where(v => v.Estado != "Inactivo");
            ViewBag.VehiculosList = new SelectList(vehiculos, "Id", "Modelo");

            // Llena la lista de operadores para el dropdown
            OperadorVM operadorVM = new()
            {
                Operador = new Operador(),
                OperadoresList = _unitOfWork.Operador.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Cedula.ToString()
                }).ToList()
            };

            // Si el id es nulo o 0, se trata de una creación
            if (id == null || id == 0)
            {
                // Retorna la vista para crear un nuevo operador con un modelo vacío
                return View(operadorVM);
            }
            else
            {
                // Si se trata de una actualización, obtiene la información del operador
                // desde el repositorio y la retorna a la vista
                operadorVM.Operador = _unitOfWork.Operador.Get(u => u.Cedula == id);
                if (operadorVM.Operador == null)
                {
                    // Si no se encuentra el operador, retorna NotFound
                    return NotFound();
                }
                return View(operadorVM);
            }
        }

        // Upsert: Maneja la creación o actualización del operador
        [HttpPost]
        public IActionResult Upsert(OperadorVM operadorVM, bool redirigirAsignar)
        {
            // Verifica si el modelo es válido
            if (ModelState.IsValid)
            {
                // Si el id es 0, se trata de una creación
                var operadorExistente = _unitOfWork.Operador.Get(u => u.Cedula == operadorVM.Operador.Cedula);

                if (operadorExistente == null)
                {
                    // Si no existe, se crea un nuevo operador
                    _unitOfWork.Operador.Add(operadorVM.Operador);
                }
                else
                {
                    // Si ya existe, se actualiza el operador existente
                    operadorExistente.Nombre = operadorVM.Operador.Nombre;
                    operadorExistente.Telefono = operadorVM.Operador.Telefono;
                    operadorExistente.TipoColaborador = operadorVM.Operador.TipoColaborador;
                }

                // Crea un registro en el historial de los operadores y las maquinas
                //RegistroOperadore registro = new RegistroOperadore
                //{
                //    OperadorCedula = operadorVM.Operador.Cedula,
                //    FechaInicio = DateTime.Today,
                //    FechaFin = DateTime.Today, // Asigna una fecha de fin por defecto
                //    VehiculoId = operadorVM.VehiculoId
                //};
                //_unitOfWork.RegistroOperadores.Add(registro);

                _unitOfWork.Save();

                if (redirigirAsignar)
                {
                    return RedirectToAction("Upsert", "RegistroOperadores");
                }

                return RedirectToAction("Index");
            }

            // Si se trata de una creacion nueva, trae los vehiculos que no esten inactivos
            // y los asigna al ViewBag para el dropdown
            var vehiculos = _unitOfWork.Vehiculo.GetAll().Where(v => v.Estado != "Inactivo");
            ViewBag.VehiculosList = new SelectList(vehiculos, "Id", "Modelo");

            // Retorna la vista con el modelo de los vehiculos para seleccionarlos y asignarlo
            // a un operador
            return View(operadorVM);
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }

                var operador = _unitOfWork.Operador.Get(u => u.Cedula == id);
                if (operador == null)
                {
                    return NotFound();
                }

                //soft delete
                operador.TipoColaborador = "Inactivo"; // Cambiar estado a Inactivo
                _unitOfWork.Operador.Update(operador);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Se ha inactivado exitosamente" });
            }
            catch
            {
                return Json(new { success = false, message = "No se pudo inactivar" });
            }
        }

        [HttpPost]
        public IActionResult Activar(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }

                var operador = _unitOfWork.Operador.Get(u => u.Cedula == id);
                if (operador == null)
                {
                    return NotFound();
                }

                operador.TipoColaborador = "Externo";
                _unitOfWork.Operador.Update(operador);
                _unitOfWork.Save();

                return Json(new { success = true, message = "Se ha activado exitosamente" });
            }
            catch
            {
                return Json(new { success = false, message = "No se pudo activar" });
            }
        }


        // Acciones para manejar los documentos de los operadores
        //permite enviar a la el modelo con el operador al asignar documentos
        [HttpGet]
        public IActionResult CargarDocumentoOperador(int id)
        {
            DocumentoOperadorVM documentoOperadorVM = new()
            {
                DocumentoOperador = new DocumentoOperador()
                {
                    OperadorCedula = id
                }
            };

            return View(documentoOperadorVM);
        }

        // Carga un documento para un operador específico
        [HttpPost]
        public IActionResult CargarDocumentoOperador(DocumentoOperadorVM documentoOperadorVM)
        {
            // Verifica si el modelo es válido
            if (ModelState.IsValid)
            {
                // Verifica si se ha subido un archivo
                if (documentoOperadorVM.Archivo != null && documentoOperadorVM.Archivo.Length > 0)
                {
                    // Genera un nombre único para el archivo
                    var nombreArchivo = Path.GetFileNameWithoutExtension(documentoOperadorVM.Archivo.FileName);
                    var extension = Path.GetExtension(documentoOperadorVM.Archivo.FileName);
                    var nombreUnico = $"{nombreArchivo}_{DateTime.Now.Ticks}{extension}";
                    var rutaGuardar = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documentos", "documentoOperador", nombreUnico);

                    // Asegura que el directorio exista
                    using (var stream = new FileStream(rutaGuardar, FileMode.Create))
                    {
                        // Copia el archivo subido al directorio especificado
                        documentoOperadorVM.Archivo.CopyTo(stream);
                    }

                    // Guardar el path del archivo en la base de datos
                    documentoOperadorVM.DocumentoOperador.Ruta = "/documentos/documentoOperador/" + nombreUnico;
                }

                // Guardar el registro en base de datos
                _unitOfWork.DocumentoOperador.Add(documentoOperadorVM.DocumentoOperador);
                _unitOfWork.Save();

                return RedirectToAction("DocumentoOperador", new { id = documentoOperadorVM.DocumentoOperador.OperadorCedula });
            }

            return View(documentoOperadorVM);
        }

        // Muestra los documentos de un operador específico
        [HttpGet]
        public IActionResult DocumentoOperador(int id)
        {
            // Verifica si el operador existe
            var operador = _unitOfWork.Operador.Get(u => u.Cedula == id);

            if (operador == null)
            {
                // Si no se encuentra el operador, retorna NotFound
                return NotFound();
            }

            // Crea una instancia del ViewModel OperadorVM con el operador encontrado
            OperadorVM operadorVM = new OperadorVM
            {
                Operador = operador
            };

            // Obtiene la informacion del operador y los asigna al ViewModel
            return View(operadorVM);
        }

        // Obtiene los documentos de un operador específico
        //obtiene un id para identificar el operador
        [HttpGet]
        public IActionResult GetDocumentosOperador(int id)
        {
            // Verifica si el operador existe
            //obtiene los documentos del operador por su cédula
            var documentos = _unitOfWork.DocumentoOperador
                .GetAll()
                .Where(d => d.OperadorCedula == id)
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

        // Elimina un documento de un operador específico
        [HttpDelete]
        public IActionResult DeleteDocumento(int id)
        {
            // Busca el documento por su ID
            var documento = _unitOfWork.DocumentoOperador.Get(d => d.Id == id);
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
            _unitOfWork.DocumentoOperador.Remove(documento);
            _unitOfWork.Save();

            // Retorna un mensaje de éxito al eliminar el documento
            return Json(new { success = true, message = "Documento eliminado exitosamente" });
        }

        // Registro de operadores: Muestra la vista para registrar operadores
        [HttpGet]
        public IActionResult RegistroOperadores()
        {
            return View();
        }

        // Obtiene todos los registros de operadores y los devuelve en formato JSON
        [HttpGet]
        public IActionResult GetRegistroOperadores()
        {
            // Obtiene todos los registros de operadores desde el repositorio
            var registros = _unitOfWork.RegistroOperadores.GetAll().Select(o => new
            {
                o.OperadorCedula,
                OperadorNombre = _unitOfWork.Operador.Get(x => x.Cedula == o.OperadorCedula).Nombre,
                VehiculoModelo = _unitOfWork.Vehiculo.Get(x => x.Id == o.VehiculoId).Modelo,
                Fecha = o.FechaInicio.ToString("dd/MM/yyyy"),
                o.VehiculoId,
            }).ToList();
            // Retorna los registros en formato JSON
            return Json(new { data = registros });
        }

    }
}