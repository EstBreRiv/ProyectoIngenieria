using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoIngenieria.Models;

public partial class DocumentoOperador
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio.")]
    public string Nombre { get; set; } = null!;

    public string Ruta { get; set; } = null!;

    public int OperadorCedula { get; set; }

    public virtual Operador OperadorCedulaNavigation { get; set; } = null!;
}
