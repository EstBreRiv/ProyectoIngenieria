using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class DocumentoVehiculo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Ruta { get; set; } = null!;

    public int VehiculoId { get; set; }

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
