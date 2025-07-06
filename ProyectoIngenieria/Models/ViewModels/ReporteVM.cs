namespace ProyectoIngenieria.Models.ViewModels
{
    public class ReporteVM
    {
        public int vehiculoId { get; set; }
        public string modelo { get; set; }

        public string placa { get; set; }
        public string mes { get; set; }

        public decimal totalHoras { get; set; }

        public decimal ingresoTotal { get; set; }

        public decimal gastoCombustible { get; set; }

        public decimal gastoMantenimiento { get; set; }

        public decimal totalGastos { get; set; }

        public decimal utilidad { get; set; }

    }
}
