using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class RepuestosMantenimiento
{
    public int Id { get; set; }

    public int CatalogoRepuestoId { get; set; }

    public int RegistroMantenimientoId { get; set; }

    public virtual CatalogoRepuesto CatalogoRepuesto { get; set; } = null!;

    public virtual RegistroMantenimiento RegistroMantenimiento { get; set; } = null!;
}
