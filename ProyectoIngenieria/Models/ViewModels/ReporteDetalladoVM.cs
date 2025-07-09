namespace ProyectoIngenieria.Models.ViewModels
{
    public class ReporteDetalladoVM
    {
        public int VehiculoId { get; set; }

        public string Marca { get; set; } = string.Empty;

        public string Modelo { get; set; } = string.Empty;

        public string Placa { get; set; } = string.Empty;

        public string Fecha_reporte { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");

        public string Periodo { get; set; } = string.Empty;

        public DateOnly fechaInicio { get; set; }

        public DateOnly fechaFin { get; set; }

        public decimal TotalIngresos { get; set; } = 0;

        public decimal TotalHorasTrabajo { get; set; }

        public decimal GastoMantenimiento { get; set; }
        public decimal GastoCombustible { get; set; }

        public decimal Utilidad { get; set; }


        public List<RegistroMantenimiento> Mantenimientos { get; set; } = new List<RegistroMantenimiento>();
        public List<RegistroCombustible> Combustibles { get; set; } = new List<RegistroCombustible>();
        public List<HorasTrabajo> HorasTrabajos { get; set; } = new List<HorasTrabajo>();
    }
}
