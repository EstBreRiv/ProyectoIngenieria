using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class RegistroCombustible
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Se debe digitar la fecha de compra")]
    public DateOnly FechaCompra { get; set; }

    [Required(ErrorMessage = "Se deben especificar los litros comprados")]
    public decimal LitrosComprados { get; set; }

    public decimal PrecioLitro { get; set; }

    public decimal TotalPagado { get; set; }

    public int VehiculoId { get; set; }

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
