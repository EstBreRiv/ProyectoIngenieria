using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class Vehiculo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Se debe ingresar el modelo del vehículo.")]
    [StringLength(50, ErrorMessage = "El modelo no puede tener más de cincuenta caracteres.")]
    public string Modelo { get; set; } = null!;

    public string Estado { get; set; } = null!;

    [Required(ErrorMessage = "Se debe ingresar una descripción del vehículo.")]
    [StringLength(100, ErrorMessage = "La descripción no puede tener más de cien caracteres.")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "Se debe ingresar el número de serie del vehículo.")]
    [StringLength(20, ErrorMessage = "La placa no puede tener más de 20 caracteres.")]
    public string Placa { get; set; }

    [Required(ErrorMessage = "Se debe seleccionar la empresa a la que pertenece el vehículo.")]
    public int EmpresaId { get; set; }

    [Required(ErrorMessage = "Se debe seleccionar la marca del vehículo.")]
    public int MarcaId { get; set; }

    [Required(ErrorMessage = "Se debe seleccionar el tipo de vehículo.")]
    public int TipoVehiculoId { get; set; }

    public virtual ICollection<DocumentoVehiculo> DocumentoVehiculos { get; set; } = new List<DocumentoVehiculo>();

    public virtual Empresa Empresa { get; set; } = null!;

    public virtual ICollection<HorasTrabajo> HorasTrabajos { get; set; } = new List<HorasTrabajo>();

    public virtual Marca Marca { get; set; } = null!;

    public virtual ICollection<Notificacion> Notificacions { get; set; } = new List<Notificacion>();

    public virtual ICollection<RegistroCombustible> RegistroCombustibles { get; set; } = new List<RegistroCombustible>();

    public virtual ICollection<RegistroMantenimiento> RegistroMantenimientos { get; set; } = new List<RegistroMantenimiento>();

    public virtual ICollection<RegistroOperadore> RegistroOperadores { get; set; } = new List<RegistroOperadore>();

    public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();

    public virtual TipoVehiculo TipoVehiculo { get; set; } = null!;
}
