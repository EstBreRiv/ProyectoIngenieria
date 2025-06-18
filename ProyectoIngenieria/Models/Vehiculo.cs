using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class Vehiculo
{
    public int Id { get; set; }

    public string Modelo { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Placa { get; set; }

    public int EmpresaId { get; set; }

    public int MarcaId { get; set; }

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
