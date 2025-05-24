using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class RegistroMantenimiento
{
    public int Id { get; set; }

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public DateOnly Fecha { get; set; }

    public int VehiculoId { get; set; }

    public int CatalogoMantenimientoId { get; set; }

    public virtual CatalogoMantenimiento CatalogoMantenimiento { get; set; } = null!;

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
