using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class Operador
{
    public int Cedula { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<DocumentoOperador> DocumentoOperadors { get; set; } = new List<DocumentoOperador>();

    public virtual ICollection<RegistroMantenimiento> RegistroMantenimientos { get; set; } = new List<RegistroMantenimiento>();

    public virtual ICollection<RegistroOperadore> RegistroOperadores { get; set; } = new List<RegistroOperadore>();
}
