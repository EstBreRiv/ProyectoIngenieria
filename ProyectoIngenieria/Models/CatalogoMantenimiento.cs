using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class CatalogoMantenimiento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<RegistroMantenimiento> RegistroMantenimientos { get; set; } = new List<RegistroMantenimiento>();
}
