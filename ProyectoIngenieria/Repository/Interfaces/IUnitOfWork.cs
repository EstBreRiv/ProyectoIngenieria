namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IEmpresaRepository Empresa { get; }
        IVehiculosRepository Vehiculo { get; }
        IOperadoresRepository Operador { get; }
        ICatalogoMantenimientoRepository CatalogoMantenimiento { get; }
        IDocumentoOperadorRepository DocumentoOperador { get; }
        IDocumentoVehiculoRepository DocumentoVehiculo { get; }
        IHorasTrabajoRepository HorasTrabajo { get; }
        INotificacionRepository Notificacion { get; }
        IRegistroCombustibleRepository RegistroCombustible { get; }
        IRegistroMantenimientoRepository RegistroMantenimiento { get; }
        IProyectoRepository Proyecto { get; }
        ILugarTrabajoRepository LugarTrabajo { get; }
        ITipoTrabajoRepository TipoTrabajo { get; }
        IRegistroOperadoresRepository RegistroOperadores { get; }
        IMarcaRepository Marca { get; }
        ITipoVehiculoRepository TipoVehiculo { get; }
        //Cambiar a catalogo de repuesto
        IRepuestoRepository Repuesto { get; }
        IRepuestosMantenimientoRepository RepuestosMantenimiento { get; }
        IOperadorMantenimientoRepository OperadorMantenimiento { get; }
        object EmpresaRepository { get; }

        void Save();
    }
}
