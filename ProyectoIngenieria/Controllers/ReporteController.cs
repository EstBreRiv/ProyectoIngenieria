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
        public IActionResult GetReporteData(int mes)
        {
            

            if (mes < 1 || mes > 12)
            {
                return BadRequest("El mes debe estar entre 1 y 12.");
            }

            List<ReporteVM> reportes = new List<ReporteVM>();

            var vehiculos = _unitOfWork.Vehiculo.GetAll();

            foreach (var vehiculo in vehiculos)
            {
                // Obtener el total de mantenimientos del vehículo
                var totalMantenimientos = _unitOfWork.RegistroMantenimiento.GetAll().
                    Where(m => m.VehiculoId == vehiculo.Id).Where(x => x.Fecha.Month == mes);

                var costosMantenimientos = totalMantenimientos.Sum(m => m.Precio);

                var gastoCombustible = _unitOfWork.RegistroCombustible.GetAll()
                    .Where(c => c.VehiculoId == vehiculo.Id && c.FechaCompra.Month == mes)
                    .Sum(c => c.TotalPagado);

                var ingresoHoras = _unitOfWork.HorasTrabajo.GetAll().
                    Where(c => c.VehiculoId == vehiculo.Id && c.Fecha.Month == mes).Sum(n => n.TotalGanancia);

                ReporteVM reporteVM = new ReporteVM
                {
                    modelo = vehiculo.Modelo,
                    placa = vehiculo.Placa,
                    mes = meses[mes - 1],
                    totalHoras = _unitOfWork.HorasTrabajo.GetAll()
                        .Where(c => c.VehiculoId == vehiculo.Id && c.Fecha.Month == mes)
                        .Sum(n => n.TotalHoras),
                    ingresoTotal = ingresoHoras,
                    gastoCombustible = gastoCombustible,
                    gastoMantenimiento = costosMantenimientos,
                    totalGastos = gastoCombustible + costosMantenimientos,
                    utilidad = ingresoHoras - (gastoCombustible + costosMantenimientos)
                };
                // Agregar el reporte a la lista
                reportes.Add(reporteVM);
            }


            //retorna la lista de reportes en formato json
            return Json(new { data = reportes });
        }
    }
}
