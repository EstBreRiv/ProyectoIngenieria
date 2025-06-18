using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class Reporte
{
    public int Id { get; set; }

    public DateOnly PeriodoInicio { get; set; }

    public DateOnly PeriodoFin { get; set; }

    public decimal TotalHoras { get; set; }

    public decimal IngresoTotal { get; set; }

    public decimal GastoCombustible { get; set; }

    public decimal GastoMantenimiento { get; set; }

    public decimal Utilidad { get; set; }

    public DateOnly FechaReporte { get; set; }

    public int VehiculoId { get; set; }

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
