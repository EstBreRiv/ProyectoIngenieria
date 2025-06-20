using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class LugarTrabajo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Se debe ingresar el nombre del lugar.")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de cien caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Se debe ingresar la provincia del lugar.")]
    [StringLength(50, ErrorMessage = "La provincia no puede tener más de cincuenta caracteres.")]
    public string Provincia { get; set; } = null!;

    [Required(ErrorMessage = "Se debe ingresar el cantón del lugar.")]
    [StringLength(100, ErrorMessage = "El cantón no puede tener más de cien caracteres.")]
    public string Canton { get; set; } = null!;

    public virtual ICollection<HorasTrabajo> HorasTrabajos { get; set; } = new List<HorasTrabajo>();
}
