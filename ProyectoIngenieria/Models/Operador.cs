using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class Operador
{
    public int Cedula { get; set; }

    public string Nombre { get; set; } = null!;

    public int VehiculoId { get; set; }

    public virtual ICollection<DocumentoOperador> DocumentoOperadors { get; set; } = new List<DocumentoOperador>();

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
