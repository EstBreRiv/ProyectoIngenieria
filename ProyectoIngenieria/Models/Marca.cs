using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class Marca
{
    public int Id { get; set; }

    public string NombreMarca { get; set; } = null!;

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
