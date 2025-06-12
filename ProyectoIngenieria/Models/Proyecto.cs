using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class Proyecto
{
    public int Id { get; set; }

    public int NombreProyecto { get; set; }

    public int Cliente { get; set; }

    public int FechaInicio { get; set; }

    public virtual ICollection<HorasTrabajo> HorasTrabajos { get; set; } = new List<HorasTrabajo>();
}
