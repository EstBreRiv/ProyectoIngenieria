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

    public virtual ICollection<DocumentoOperador> DocumentoOperadors { get; set; } = new List<DocumentoOperador>();

    public virtual ICollection<RegistroOperadores> RegistroOperadores { get; set; } = new List<RegistroOperadores>();
}
