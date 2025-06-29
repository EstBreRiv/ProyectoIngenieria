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

            List<ReporteVM> reportes = new List<ReporteVM>();

            var vehiculos = _unitOfWork.Vehiculo.GetAll();

            foreach (var vehiculo in vehiculos)
            {
                // Obtener mantenimientos dentro del rango
                var totalMantenimientos = _unitOfWork.RegistroMantenimiento.GetAll()
                    .Where(m => m.VehiculoId == vehiculo.Id && m.Fecha >= fechaInicio && m.Fecha <= fechaFin);

                var costosMantenimientos = totalMantenimientos.Sum(m => m.Precio);

                var gastoCombustible = _unitOfWork.RegistroCombustible.GetAll()
                    .Where(c => c.VehiculoId == vehiculo.Id && c.FechaCompra >= fechaInicio && c.FechaCompra <= fechaFin)
                    .Sum(c => c.TotalPagado);

                var ingresoHoras = _unitOfWork.HorasTrabajo.GetAll()
                    .Where(h => h.VehiculoId == vehiculo.Id && h.Fecha >= fechaInicio && h.Fecha <= fechaFin)
                    .Sum(h => h.TotalGanancia);

                var totalHoras = _unitOfWork.HorasTrabajo.GetAll()
                    .Where(h => h.VehiculoId == vehiculo.Id && h.Fecha >= fechaInicio && h.Fecha <= fechaFin)
                    .Sum(h => h.TotalHoras);

                ReporteVM reporteVM = new ReporteVM
                {
                    modelo = vehiculo.Modelo,
                    placa = vehiculo.Placa,
                    mes = $"{fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}",  // Mostrar rango de fechas
                    totalHoras = totalHoras,
                    ingresoTotal = ingresoHoras,
                    gastoCombustible = gastoCombustible,
                    gastoMantenimiento = costosMantenimientos,
                    totalGastos = gastoCombustible + costosMantenimientos,
                    utilidad = ingresoHoras - (gastoCombustible + costosMantenimientos)
                };

                reportes.Add(reporteVM);
            }

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
