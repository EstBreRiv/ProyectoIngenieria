using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class RegistroMantenimiento
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Se debe ingresar una descripción del mantenimiento realizado.")]
    [StringLength(200, ErrorMessage = "La descripción no puede tener más de doscientos caracteres.")]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "Se debe digitar el precio del mantenimiento.")]
    [Range(0, 999999999999999999, ErrorMessage = "El precio debe ser positivo")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "Se debe proporcionar la fecha del mantenimiento.")]
    public DateOnly Fecha { get; set; }

    [Required(ErrorMessage = "Se debe seleccionar un vehículo.")]
    public int VehiculoId { get; set; }

    public virtual ICollection<OperadorMantenimiento> OperadorMantenimientos { get; set; } = new List<OperadorMantenimiento>();

    public virtual ICollection<RepuestosMantenimiento> RepuestosMantenimientos { get; set; } = new List<RepuestosMantenimiento>();

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
