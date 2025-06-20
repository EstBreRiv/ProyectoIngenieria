using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class TipoVehiculo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Se debe ingresar el nombre del tipo de vehículo.")]
    [StringLength(50, ErrorMessage = "El nombre no puede tener más de cincuenta caracteres.")]
    public string Tipo { get; set; } = null!;

    [Required(ErrorMessage = "Se debe ingresar la descripción tipo de vehículo.")]
    [StringLength(100, ErrorMessage = "La descripción no puede tener más de cien caracteres.")]
    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
