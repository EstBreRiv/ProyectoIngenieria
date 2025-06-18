using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class Proyecto
{
    public int Id { get; set; }

    public string NombreProyecto { get; set; } = null!;

    public string Cliente { get; set; } = null!;

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public virtual ICollection<HorasTrabajo> HorasTrabajos { get; set; } = new List<HorasTrabajo>();
}
