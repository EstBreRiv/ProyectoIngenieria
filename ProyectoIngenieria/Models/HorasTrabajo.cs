using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class HorasTrabajo
{
    public int Id { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal HorometroInicial { get; set; }

    public decimal HorometroFinal { get; set; }

    public string Lugar { get; set; } = null!;

    public decimal PrecioHora { get; set; }

    public int VehiculoId { get; set; }

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
