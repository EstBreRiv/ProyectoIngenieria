using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class CatalogoRepuesto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Se debe ingresar un nombre al repuesto.")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de cien caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Se debe ingresar una descripción al repuesto.")]
    [StringLength(100, ErrorMessage = "La descripción no puede tener más de cien caracteres.")]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "Se debe digitar un precio.")]
    [Range(0, 999999999999999999, ErrorMessage = "El precio debe ser un valor positivo.")]
    public decimal PrecioEstimado { get; set; }

    public virtual ICollection<RepuestosMantenimiento> RepuestosMantenimientos { get; set; } = new List<RepuestosMantenimiento>();
}
