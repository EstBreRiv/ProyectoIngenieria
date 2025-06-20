using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class Proyecto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Se debe ingresar el nombre del proyecto.")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de cien caracteres.")]
    public string NombreProyecto { get; set; } = null!;

    [Required(ErrorMessage = "Se debe ingresar el cliente del proyecto.")]
    [StringLength(50, ErrorMessage = "El cliente no puede tener más de cincuenta caracteres.")]
    public string Cliente { get; set; } = null!;

    [Required(ErrorMessage = "Se proporcionar una fecha de inicio.")]
    public DateOnly FechaInicio { get; set; }

    [Required(ErrorMessage = "Se proporcionar una fecha de fin.")]
    public DateOnly FechaFin { get; set; }

    public virtual ICollection<HorasTrabajo> HorasTrabajos { get; set; } = new List<HorasTrabajo>();
}
