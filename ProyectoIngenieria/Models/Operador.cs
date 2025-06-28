using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class Operador
{
    [Required(ErrorMessage = "Se debe agregar un número de cédula.")]
    [Range(100000000, 999999999, ErrorMessage = "La cédula debe contener nueve dígitos.")]
    public int Cedula { get; set; }

    [Required(ErrorMessage = "Se debe agregar un nombre.")]
    [StringLength(50, ErrorMessage = "El nombre no puede tener más de cincuenta caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Se debe agregar un número de teléfono.")]
    [Range(10000000, 99999999, ErrorMessage = "El número de teléfono debe contener ocho dígitos.")]
    public string Telefono { get; set; } = null!;

    [Required(ErrorMessage = "Se debe seleccionar un tipo de colaborador.")]
    public string TipoColaborador { get; set; } = null!;

    public virtual ICollection<DocumentoOperador> DocumentoOperadors { get; set; } = new List<DocumentoOperador>();

    public virtual ICollection<OperadorMantenimiento> OperadorMantenimientos { get; set; } = new List<OperadorMantenimiento>();

    public virtual ICollection<RegistroOperadore> RegistroOperadores { get; set; } = new List<RegistroOperadore>();
}
