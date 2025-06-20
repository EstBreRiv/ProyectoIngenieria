using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class TipoTrabajo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Se debe ingresar el nombre del tipo de trabajo.")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de cien caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Se debe ingresar la descripción tipo de trabajo.")]
    [StringLength(100, ErrorMessage = "La descripción no puede tener más de cien caracteres.")]
    public string Descripcion { get; set; } = null!;

    public virtual ICollection<HorasTrabajo> HorasTrabajos { get; set; } = new List<HorasTrabajo>();
}
