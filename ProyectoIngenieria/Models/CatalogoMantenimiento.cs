using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class CatalogoMantenimiento
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Se debe ingresar un nombre al mantenimiento.")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de cien caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Se debe ingresar una descripción al mantenimiento.")]
    [StringLength(200, ErrorMessage = "La descripción no puede tener más de doscientos caracteres.")]
    public string Descripcion { get; set; } = null!;

    public virtual ICollection<OperadorMantenimiento> OperadorMantenimientos { get; set; } = new List<OperadorMantenimiento>();
}
