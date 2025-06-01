using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class Operador
{
    [Required(ErrorMessage = "El campo Cédula es obligatorio.")]
    public int Cedula { get; set; }

    [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Se debe elegir un vehículo para operar.")]
    public int VehiculoId { get; set; }

    public virtual ICollection<DocumentoOperador> DocumentoOperadors { get; set; } = new List<DocumentoOperador>();

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
