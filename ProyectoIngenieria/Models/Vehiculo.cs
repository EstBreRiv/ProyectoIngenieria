using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class Vehiculo
{
    [ValidateNever]
    public int Id { get; set; }
    [ValidateNever]
    [StringLength(100)]
    [RegularExpression(@"^[a-zA-Z0-9\sáéíóúñÑ.,-]+$",
     ErrorMessage = "Caracteres no permitidos")]
    public string Modelo { get; set; } = null!;
    [ValidateNever]
    public string Estado { get; set; } = null!;

    [ValidateNever]
    public string? Descripcion { get; set; }

    [ValidateNever]
    public string? Placa { get; set; }


    [ValidateNever]

    public string Tipo { get; set; } = null!;
    [ValidateNever]

    public int EmpresaId { get; set; }
    [ValidateNever]

    public virtual ICollection<DocumentoVehiculo> DocumentoVehiculos { get; set; } = new List<DocumentoVehiculo>();
    [ValidateNever]

    public virtual Empresa Empresa { get; set; } = null!;
    [ValidateNever]

    public virtual ICollection<HorasTrabajo> HorasTrabajos { get; set; } = new List<HorasTrabajo>();
    [ValidateNever]

    public virtual ICollection<Notificacion> Notificacions { get; set; } = new List<Notificacion>();
    [ValidateNever]

    public virtual ICollection<Operador> Operadors { get; set; } = new List<Operador>();
    [ValidateNever]

    public virtual ICollection<RegistroCombustible> RegistroCombustibles { get; set; } = new List<RegistroCombustible>();
    [ValidateNever]

    public virtual ICollection<RegistroMantenimiento> RegistroMantenimientos { get; set; } = new List<RegistroMantenimiento>();
}
