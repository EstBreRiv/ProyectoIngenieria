using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class Marca
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Se debe ingresar un nombre de marca.")]
    [StringLength(50, ErrorMessage = "El nombre no puede tener más de cincuenta caracteres.")]
    public string NombreMarca { get; set; } = null!;

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
