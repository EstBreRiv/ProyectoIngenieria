using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models
{
    public partial class HorasTrabajo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateOnly Fecha { get; set; }

        [Required(ErrorMessage = "Se debe ingresar el horómetro inicial.")]
        [Range(0, double.MaxValue, ErrorMessage = "El horómetro inicial debe ser positivo")]
        public decimal HorometroInicial { get; set; }

        [Required(ErrorMessage = "Se debe ingresar el horómetro final.")]
        [Range(0, double.MaxValue, ErrorMessage = "El horómetro final debe ser mayor que 0")]
        public decimal HorometroFinal { get; set; }

        [Required(ErrorMessage = "Se debe ingresar el precio por hora.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio por hora debe ser mayor que 0.")]
        public decimal PrecioHora { get; set; }

        public decimal TotalHoras { get; set; }

        public decimal TotalGanancia { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un lugar de trabajo.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un lugar de trabajo válido.")]
        public int LugarTrabajoId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de trabajo.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de trabajo válido.")]
        public int TipoTrabajoId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un proyecto.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un proyecto válido.")]
        public int ProyectoId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un vehículo.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un vehículo válido.")]
        public int VehiculoId { get; set; }


        public virtual LugarTrabajo? LugarTrabajo { get; set; }
        public virtual Proyecto? Proyecto { get; set; }
        public virtual TipoTrabajo? TipoTrabajo { get; set; }
        public virtual Vehiculo? Vehiculo { get; set; }

    }
}
