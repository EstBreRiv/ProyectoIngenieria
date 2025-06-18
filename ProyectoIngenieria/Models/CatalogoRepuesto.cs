using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class CatalogoRepuesto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal PrecioEstimado { get; set; }

    public virtual ICollection<RepuestosMantenimiento> RepuestosMantenimientos { get; set; } = new List<RepuestosMantenimiento>();
}
