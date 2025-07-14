using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class RegistroCombustible
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Se debe proporcionar una fecha de compra.")]
    public DateOnly FechaCompra { get; set; }

    [Required(ErrorMessage = "Se deben digitar los litros comprados.")]
    [Range(0.01, 999999999999999999, ErrorMessage = "La cantidad de litros debe ser mayor a 0.")]
    public decimal LitrosComprados { get; set; }

    [Required(ErrorMessage = "Se deben digitar el precio por litro.")]
    [Range(0.01, 999999999999999999, ErrorMessage = "El precio debe ser mayor a 0.")]
    public decimal PrecioLitro { get; set; }

    [Required(ErrorMessage = "No se puede calcular el total.")]
    public decimal TotalPagado { get; set; }

    [Required(ErrorMessage = "Se debe seleccionar un vehículo.")]
    public int VehiculoId { get; set; }

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
