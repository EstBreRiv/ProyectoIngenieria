using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class RegistroOperadore
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public int OperadorCedula { get; set; }

    public int VehiculoId { get; set; }

    public virtual Operador OperadorCedulaNavigation { get; set; } = null!;

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
