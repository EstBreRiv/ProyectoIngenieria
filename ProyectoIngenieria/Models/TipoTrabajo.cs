using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class TipoTrabajo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<HorasTrabajo> HorasTrabajos { get; set; } = new List<HorasTrabajo>();
}
