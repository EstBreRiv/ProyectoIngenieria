using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class TipoVehiculo
{
    public int Id { get; set; }

    public string Tipo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
