using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class OperadorMantenimiento
{
    public int Id { get; set; }

    public decimal HorasTrabajo { get; set; }

    public int OperadorCedula { get; set; }

    public int CatalogoMantenimientoId { get; set; }

    public int RegistroMantenimientoId { get; set; }

    public virtual CatalogoMantenimiento CatalogoMantenimiento { get; set; } = null!;

    public virtual Operador OperadorCedulaNavigation { get; set; } = null!;

    public virtual RegistroMantenimiento RegistroMantenimiento { get; set; } = null!;
}
