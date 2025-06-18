using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class HorasTrabajo
{
    public int Id { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal HorometroInicial { get; set; }

    public decimal HorometroFinal { get; set; }

    public decimal PrecioHora { get; set; }

    public decimal TotalHoras { get; set; }

    public decimal TotalGanancia { get; set; }

    public int VehiculoId { get; set; }

    public int LugarTrabajoId { get; set; }

    public int TipoTrabajoId { get; set; }

    public int ProyectoId { get; set; }

    public virtual LugarTrabajo LugarTrabajo { get; set; } = null!;

    public virtual Proyecto Proyecto { get; set; } = null!;

    public virtual TipoTrabajo TipoTrabajo { get; set; } = null!;

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
