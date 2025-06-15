using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class Vehiculo
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El campo modelo es obligatorio")]
    [StringLength(100)]
    [RegularExpression(@"^[a-zA-Z0-9\sáéíóúñÑ.,-]+$",
     ErrorMessage = "Caracteres especiales no permitidos")]
    public string Modelo { get; set; } = null!;

    [Required]
    public string Estado { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Placa { get; set; }

    [Required(ErrorMessage = "Se debe seleccionar un tipo")]
    public string Tipo { get; set; } = null!;

    [Required(ErrorMessage = "Se debe seleccionar una empresa")]
    public int EmpresaId { get; set; }

    public virtual ICollection<DocumentoVehiculo> DocumentoVehiculos { get; set; } = new List<DocumentoVehiculo>();

    public virtual Empresa Empresa { get; set; } = null!;

    public virtual ICollection<HorasTrabajo> HorasTrabajos { get; set; } = new List<HorasTrabajo>();

    public virtual ICollection<Notificacion> Notificacions { get; set; } = new List<Notificacion>();

    public virtual ICollection<Operador> Operadors { get; set; } = new List<Operador>();

    public virtual ICollection<RegistroCombustible> RegistroCombustibles { get; set; } = new List<RegistroCombustible>();

    public virtual ICollection<RegistroMantenimiento> RegistroMantenimientos { get; set; } = new List<RegistroMantenimiento>();

    public virtual ICollection<RegistroOperadores> RegistroOperadores { get; set; } = new List<RegistroOperadores>();

}
