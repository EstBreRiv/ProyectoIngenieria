using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class LugarTrabajo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Provincia { get; set; } = null!;

    public string Canton { get; set; } = null!;

    public virtual ICollection<HorasTrabajo> HorasTrabajos { get; set; } = new List<HorasTrabajo>();
}
