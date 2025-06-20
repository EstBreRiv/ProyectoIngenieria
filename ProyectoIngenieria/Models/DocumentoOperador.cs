using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class DocumentoOperador
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Se debe ingresar un nombre para el documento.")]
    [StringLength(50, ErrorMessage = "El nombre no puede tener más de cincuenta caracteres.")]
    public string Nombre { get; set; } = null!;

    public string Ruta { get; set; } = null!;

    public int OperadorCedula { get; set; }

    public virtual Operador OperadorCedulaNavigation { get; set; } = null!;
}
