using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;
using ProyectoIngenieria.Services;

namespace ProyectoIngenieria.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ProyectoIngenieria.Utilities.RolesUsuario.Role_Admin)]

    public class ReporteController : Controller
    {
        // Inyección de dependencias del UnitOfWork para acceder a los repositorios
        private readonly IUnitOfWork _unitOfWork;
        private readonly PdfService _pdfService;

        private string[] meses = new string[]
        {
            "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
            "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
        };
        // Constructor que recibe el UnitOfWork
        public ReporteController(IUnitOfWork unitOfWork, PdfService pdfService)
        {
            _unitOfWork = unitOfWork;
            _pdfService = pdfService;

        }

        // Acción para mostrar la vista principal del reporte
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Acción para obtener los datos del reporte en formato JSON
        [HttpGet]
        public IActionResult GetReporteDataByFecha(DateOnly fechaInicio, DateOnly fechaFin)
        {
            if (fechaInicio > fechaFin)
            {
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha final.");
            }

            // 1. Obtener datos agrupados por vehículo
            var mantenimientos = _unitOfWork.RegistroMantenimiento.GetAll()
                .Where(m => m.Fecha >= fechaInicio && m.Fecha <= fechaFin)
                .GroupBy(m => m.VehiculoId)
                .Select(g => new
                {
                    VehiculoId = g.Key,
                    CostoMantenimiento = g.Sum(x => x.Precio)
                }).ToList();

            var combustibles = _unitOfWork.RegistroCombustible.GetAll()
                .Where(c => c.FechaCompra >= fechaInicio && c.FechaCompra <= fechaFin)
                .GroupBy(c => c.VehiculoId)
                .Select(g => new
                {
                    VehiculoId = g.Key,
                    Gasto = g.Sum(x => x.TotalPagado)
                }).ToList();

            var horas = _unitOfWork.HorasTrabajo.GetAll()
                .Where(h => h.Fecha >= fechaInicio && h.Fecha <= fechaFin)
                .GroupBy(h => h.VehiculoId)
                .Select(g => new
                {
                    VehiculoId = g.Key,
                    TotalHoras = g.Sum(x => x.TotalHoras),
                    Ingreso = g.Sum(x => x.TotalGanancia)
                }).ToList();

            // 2. Obtener todos los vehículos (puede incluir activos/inactivos según tus reglas)
            var vehiculos = _unitOfWork.Vehiculo.GetAll().Where(x => x.TipoVehiculoId != 2 && x.Estado != "Inactivo").ToList();

            // 3. Construir el reporte
            var reportes = vehiculos.Select(v =>
            {
                var mantenimiento = mantenimientos.FirstOrDefault(m => m.VehiculoId == v.Id);
                var combustible = combustibles.FirstOrDefault(c => c.VehiculoId == v.Id);
                var hora = horas.FirstOrDefault(h => h.VehiculoId == v.Id);

                var gastoMantenimiento = mantenimiento?.CostoMantenimiento ?? 0;
                var gastoCombustible = combustible?.Gasto ?? 0;
                var ingreso = hora?.Ingreso ?? 0;
                var totalHoras = hora?.TotalHoras ?? 0;

                return new ReporteVM
                {
                    vehiculoId = v.Id,
                    modelo = v.Modelo,
                    placa = v.Placa,
                    mes = $"{fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}",
                    totalHoras = totalHoras,
                    ingresoTotal = ingreso,
                    gastoCombustible = gastoCombustible,
                    gastoMantenimiento = gastoMantenimiento,
                    totalGastos = gastoCombustible + gastoMantenimiento,
                    utilidad = ingreso - (gastoCombustible + gastoMantenimiento)
                };
            }).ToList();

            return Json(new { data = reportes });
        }



        [HttpGet]
        public IActionResult DescargarReporteCsv(DateOnly fechaInicio, DateOnly fechaFin)
        {
            if (fechaInicio > fechaFin)
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha final.");

            // 1. Cargar todos los datos filtrados por fecha de una sola vez
            var mantenimientos = _unitOfWork.RegistroMantenimiento.GetAll()
                .Where(m => m.Fecha >= fechaInicio && m.Fecha <= fechaFin)
                .ToList();

            var combustibles = _unitOfWork.RegistroCombustible.GetAll()
                .Where(c => c.FechaCompra >= fechaInicio && c.FechaCompra <= fechaFin)
                .ToList();

            var horas = _unitOfWork.HorasTrabajo.GetAll()
                .Where(h => h.Fecha >= fechaInicio && h.Fecha <= fechaFin)
                .ToList();

            var vehiculos = _unitOfWork.Vehiculo.GetAll().ToList();

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Modelo,Placa,Rango Fechas,Horas,Ingreso,Combustible,Mantenimiento,Total Gastos,Utilidad");

            foreach (var v in vehiculos)
            {
                var totalHoras = horas.Where(h => h.VehiculoId == v.Id).Sum(h => h.TotalHoras);
                var ingresoHoras = horas.Where(h => h.VehiculoId == v.Id).Sum(h => h.TotalGanancia);
                var gastoCombustible = combustibles.Where(c => c.VehiculoId == v.Id).Sum(c => c.TotalPagado);
                var costoMantenimiento = mantenimientos.Where(m => m.VehiculoId == v.Id).Sum(m => m.Precio);

                var totalGastos = gastoCombustible + costoMantenimiento;
                var utilidad = ingresoHoras - totalGastos;

                var fila = $"{v.Modelo},{v.Placa},\"{fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}\"," +
                           $"{totalHoras},{ingresoHoras},{gastoCombustible},{costoMantenimiento},{totalGastos},{utilidad}";

                sb.AppendLine(fila);
            }

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());

            return File(buffer, "text/csv", $"ReporteVehiculos_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.csv");
        }


        public IActionResult DetalleReporte()
        {
            return View();
        }

        public ReporteDetalladoVM ConstructorReporte(int? id, DateOnly fechaInicio, DateOnly fechaFin)
        {

            if (!id.HasValue || id <= 0)
                // Validar ID del vehículo y envia una excepcion
                throw new ArgumentException("El ID del vehículo debe ser un número positivo mayor que cero.");


            if (fechaInicio > fechaFin)
                // Validar fechas y envia una excepcion
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha final.");


            // Obtener vehículo con su marca incluida
            var vehiculo = _unitOfWork.Vehiculo.Get(v => v.Id == id, includeProperties: "Marca");
            if (vehiculo == null)
                // Si no se encuentra el vehículo, lanza una excepción
                throw new KeyNotFoundException($"Vehículo con ID {id} no encontrado.");

            // Mantenimientos
            var mantenimientos = _unitOfWork.RegistroMantenimiento.GetAll(
                m => m.VehiculoId == id && m.Fecha >= fechaInicio && m.Fecha <= fechaFin
            ).ToList();

            var operadorActividad = _unitOfWork.OperadorMantenimiento.GetAll(
                               om => om.RegistroMantenimiento.Fecha >= fechaInicio && om.RegistroMantenimiento.Fecha <= fechaFin,
                                              includeProperties: "CatalogoMantenimiento,OperadorCedulaNavigation"
                                                         ).ToList();

            //Recorre los operadores actividad y agrega el nombre del operador y el tipo de mantenimiento a cada registro de mantenimiento
            foreach (var mantenimiento in mantenimientos)
            {
                var operadores = operadorActividad.Where(om => om.RegistroMantenimientoId == mantenimiento.Id).ToList();
                if (operadores.Any())
                {
                    mantenimiento.OperadorMantenimientos = operadores;

                }
            }

            // Combustibles
            var combustibles = _unitOfWork.RegistroCombustible.GetAll(
                c => c.VehiculoId == id && c.FechaCompra >= fechaInicio && c.FechaCompra <= fechaFin
            ).ToList();

            // Horas trabajadas
            var horas = _unitOfWork.HorasTrabajo.GetAll(
                h => h.VehiculoId == id && h.Fecha >= fechaInicio && h.Fecha <= fechaFin,
                includeProperties: "Proyecto,LugarTrabajo,TipoTrabajo"
            ).ToList();

            // Cálculos
            decimal totalHoras = horas.Sum(h => h.TotalHoras);
            decimal totalIngreso = horas.Sum(h => h.TotalGanancia);
            decimal gastoMantenimiento = mantenimientos.Sum(m => m.Precio);
            decimal gastoCombustible = combustibles.Sum(c => c.TotalPagado);
            decimal utilidad = totalIngreso - (gastoCombustible + gastoMantenimiento);

            // Crear ViewModel
            var vm = new ReporteDetalladoVM
            {
                VehiculoId = vehiculo.Id,
                Marca = vehiculo.Marca?.NombreMarca ?? "",
                Modelo = vehiculo.Modelo,
                Placa = vehiculo.Placa,
                Fecha_reporte = DateTime.Now.ToString(),
                Periodo = $"{fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}",
                fechaInicio = fechaInicio,
                fechaFin = fechaFin,
                TotalHorasTrabajo = totalHoras,
                TotalIngresos = totalIngreso,
                GastoMantenimiento = gastoMantenimiento,
                GastoCombustible = gastoCombustible,
                Utilidad = utilidad,
                Mantenimientos = mantenimientos,
                Combustibles = combustibles,
                HorasTrabajos = horas
            };

            return vm;
        }

        [HttpGet]
        public IActionResult ReporteDetallado(int? id, DateOnly fechaInicio, DateOnly fechaFin)
        {
            ReporteDetalladoVM vm = ConstructorReporte(id, fechaInicio, fechaFin);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> DescargarPDF(int id, DateOnly fechaInicio, DateOnly fechaFin)
        {
            var vm = ConstructorReporte(id, fechaInicio, fechaFin);
            var pdfBytes = await _pdfService.GeneratePdfAsync(this, "ReporteDetallado", vm);
            return File(pdfBytes, "application/pdf", "ReporteDetallado_" + vm.Modelo + "_" + vm.Placa + ".pdf");
        }

    }
}
