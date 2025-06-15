using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class RegistroCombustible
{
    public int Id { get; set; }

    public DateOnly? FechaCompra { get; set; }

    public decimal? LitrosComprados { get; set; }

    public decimal? PrecioLitro { get; set; }

    public decimal TotalPagado { get; set; }

    public int VehiculoId { get; set; }

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
