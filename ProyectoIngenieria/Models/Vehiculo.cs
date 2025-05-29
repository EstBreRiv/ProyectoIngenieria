using System;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models;

public partial class Vehiculo
{
    public int Id { get; set; }

    public string? Modelo { get; set; }

    public string Estado { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Placa { get; set; }

    public string? Tipo { get; set; }

    public int? EmpresaId { get; set; }

    public virtual ICollection<DocumentoVehiculo> DocumentoVehiculos { get; set; } = new List<DocumentoVehiculo>();

    public virtual Empresa? Empresa { get; set; }

    public virtual ICollection<HorasTrabajo> HorasTrabajos { get; set; } = new List<HorasTrabajo>();

    public virtual ICollection<Notificacion> Notificacions { get; set; } = new List<Notificacion>();

    public virtual ICollection<Operador> Operadors { get; set; } = new List<Operador>();

    public virtual ICollection<RegistroCombustible> RegistroCombustibles { get; set; } = new List<RegistroCombustible>();

    public virtual ICollection<RegistroMantenimiento> RegistroMantenimientos { get; set; } = new List<RegistroMantenimiento>();
}
