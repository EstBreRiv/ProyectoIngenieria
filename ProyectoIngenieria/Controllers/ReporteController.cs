using Microsoft.AspNetCore.Mvc;
using ProyectoIngenieria.Models.ViewModels;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Controllers
{
    public class ReporteController : Controller
    {
        // Inyección de dependencias del UnitOfWork para acceder a los repositorios
        private readonly IUnitOfWork _unitOfWork;
        private string [] meses = new string[]
        {
            "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
            "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
        };
        // Constructor que recibe el UnitOfWork
        public ReporteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            var vehiculos = _unitOfWork.Vehiculo.GetAll().ToList();

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

            var vehiculos = _unitOfWork.Vehiculo.GetAll();

            var sb = new System.Text.StringBuilder();

            // Cabecera CSV
            sb.AppendLine("Modelo,Placa,Rango Fechas,Horas,Ingreso,Combustible,Mantenimiento,Total Gastos,Utilidad");

            foreach (var vehiculo in vehiculos)
            {
                var mantenimientos = _unitOfWork.RegistroMantenimiento.GetAll()
                    .Where(m => m.VehiculoId == vehiculo.Id && m.Fecha >= fechaInicio && m.Fecha <= fechaFin);
                var costosMantenimientos = mantenimientos.Sum(m => m.Precio);

                var gastoCombustible = _unitOfWork.RegistroCombustible.GetAll()
                    .Where(c => c.VehiculoId == vehiculo.Id && c.FechaCompra >= fechaInicio && c.FechaCompra <= fechaFin)
                    .Sum(c => c.TotalPagado);

                var ingresoHoras = _unitOfWork.HorasTrabajo.GetAll()
                    .Where(h => h.VehiculoId == vehiculo.Id && h.Fecha >= fechaInicio && h.Fecha <= fechaFin)
                    .Sum(h => h.TotalGanancia);

                var totalHoras = _unitOfWork.HorasTrabajo.GetAll()
                    .Where(h => h.VehiculoId == vehiculo.Id && h.Fecha >= fechaInicio && h.Fecha <= fechaFin)
                    .Sum(h => h.TotalHoras);

                var fila = $"{vehiculo.Modelo},{vehiculo.Placa},\"{fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}\"," +
                           $"{totalHoras},{ingresoHoras},{gastoCombustible},{costosMantenimientos}," +
                           $"{gastoCombustible + costosMantenimientos},{ingresoHoras - (gastoCombustible + costosMantenimientos)}";

                sb.AppendLine(fila);
            }

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());

            return File(buffer, "text/csv", $"ReporteVehiculos_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.csv");
        }


    }
}
