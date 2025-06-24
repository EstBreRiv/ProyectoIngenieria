using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class Notificacion
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public int VehiculoId { get; set; }

    public bool Leida { get; set; } = false;

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
