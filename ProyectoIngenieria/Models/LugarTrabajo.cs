using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class LugarTrabajo
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = null!;
    [Required]

    public string Provincia { get; set; } = null!;
    [Required]

    public string Canton { get; set; } = null!;

    public virtual ICollection<HorasTrabajo> HorasTrabajos { get; set; } = new List<HorasTrabajo>();
}
