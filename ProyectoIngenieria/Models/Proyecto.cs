using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class Proyecto
{
    public int Id { get; set; }

    public string? NombreProyecto { get; set; }

    public string? Cliente { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public virtual ICollection<HorasTrabajo> HorasTrabajos { get; set; } = new List<HorasTrabajo>();

}
