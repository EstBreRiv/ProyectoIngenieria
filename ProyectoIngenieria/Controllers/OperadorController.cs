using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;
using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Controllers
{
    public class OperadorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OperadorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var operadores = _unitOfWork.Operador.GetAll();
            return Json(new { data = operadores });
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            // Cargar vehiculos para el dropdown
            var vehiculos = _unitOfWork.Vehiculo.GetAll()
                .Where(v => v.Estado != "Inactivo");
            ViewBag.VehiculosList = new SelectList(vehiculos, "Id", "Modelo");

            OperadorVM operadorVM = new()
            {
                Operador = new Operador(),
                OperadoresList = _unitOfWork.Operador.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Cedula.ToString()
                }).ToList()
            };

            if (id == null || id == 0)
            {
                // Create
                return View(operadorVM);
            }
            else
            {
                // Update
                operadorVM.Operador = _unitOfWork.Operador.Get(u => u.Cedula == id);
                if (operadorVM.Operador == null)
                {
                    return NotFound();
                }
                return View(operadorVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(OperadorVM operadorVM)
        {
            if (ModelState.IsValid)
            {
                var operadorExistente = _unitOfWork.Operador.Get(u => u.Cedula == operadorVM.Operador.Cedula);

                if (operadorExistente == null)
                {
                    _unitOfWork.Operador.Add(operadorVM.Operador);
                }
                else
                {
                    // Actualizar campos manualmente sobre la instancia ya trackeada
                    operadorExistente.Nombre = operadorVM.Operador.Nombre;
                    operadorExistente.VehiculoId = operadorVM.Operador.VehiculoId;
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            var vehiculos = _unitOfWork.Vehiculo.GetAll().Where(v => v.Estado != "Inactivo");
            ViewBag.VehiculosList = new SelectList(vehiculos, "Id", "Modelo");

            return View(operadorVM);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var operador = _unitOfWork.Operador.Get(o => o.Cedula == id);

            if (operador == null)
            {
                return Json(new { success = false, message = "Error al borrar el operador" });
            }

            var documentos = _unitOfWork.DocumentoOperador.GetAll().Where(d => d.OperadorCedula == id).ToList();

            foreach (var doc in documentos)
            {
                var rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", doc.Ruta.TrimStart('/'));
                if (System.IO.File.Exists(rutaCompleta))
                {
                    System.IO.File.Delete(rutaCompleta);
                }

                _unitOfWork.DocumentoOperador.Remove(doc);
            }

            _unitOfWork.Operador.Remove(operador);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Operador eliminado correctamente" });
        }


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

        [HttpPost]
        public IActionResult CargarDocumentoOperador(DocumentoOperadorVM documentoOperadorVM)
        {
            if (ModelState.IsValid)
            {
                if (documentoOperadorVM.Archivo != null && documentoOperadorVM.Archivo.Length > 0)
                {
                    var nombreArchivo = Path.GetFileNameWithoutExtension(documentoOperadorVM.Archivo.FileName);
                    var extension = Path.GetExtension(documentoOperadorVM.Archivo.FileName);
                    var nombreUnico = $"{nombreArchivo}_{DateTime.Now.Ticks}{extension}";
                    var rutaGuardar = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documentos", "documentoOperador", nombreUnico);

                    using (var stream = new FileStream(rutaGuardar, FileMode.Create))
                    {
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

        [HttpGet]
        public IActionResult DocumentoOperador(int id)
        {
            var operador = _unitOfWork.Operador.Get(u => u.Cedula == id);

            if (operador == null)
            {
                return NotFound();
            }

            OperadorVM operadorVM = new OperadorVM
            {
                Operador = operador
            };

            return View(operadorVM);
        }


        [HttpGet]
        public IActionResult GetDocumentosOperador(int id)
        {
            var documentos = _unitOfWork.DocumentoOperador
                .GetAll()
                .Where(d => d.OperadorCedula == id)
                .Select(d => new {
                    d.Id,
                    d.Nombre,
                    d.Ruta
                })
                .ToList();

            return Json(new { data = documentos });
        }

        [HttpDelete]
        public IActionResult DeleteDocumento(int id)
        {
            var documento = _unitOfWork.DocumentoOperador.Get(d => d.Id == id);
            if (documento == null)
            {
                return Json(new { success = false, message = "Error al borrar el documento" });
            }

            var rutaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", documento.Ruta.TrimStart('/'));
            if (System.IO.File.Exists(rutaFisica))
            {
                System.IO.File.Delete(rutaFisica);
            }

            _unitOfWork.DocumentoOperador.Remove(documento);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Documento eliminado exitosamente" });
        }

    }
}