using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class Empresa
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Se debe ingresar el nombre de la empresa.")]
    [StringLength(50, ErrorMessage = "El nombre no puede tener más de cincuenta caracteres.")]
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
