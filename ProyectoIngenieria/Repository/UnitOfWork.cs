using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProyectoIngenieriaContext _db;

        public IEmpresaRepository Empresa { get; private set; }

        public IVehiculosRepository Vehiculo { get; private set; }

        public IOperadoresRepository Operador { get; private set; }

        public ICatalogoMantenimientoRepository CatalogoMantenimiento { get; private set; }

        public IDocumentoOperadorRepository DocumentoOperador { get; private set; }

        public IDocumentoVehiculoRepository DocumentoVehiculo { get; private set; }

        public IHorasTrabajoRepository HorasTrabajo { get; private set; }

        public INotificacionRepository Notificacion { get; private set; }

        public IRegistroCombustibleRepository RegistroCombustible { get; private set; }

        public IRegistroMantenimientoRepository RegistroMantenimiento { get; private set; }

        public IProyectoRepository Proyecto { get; private set; }

        public ILugarTrabajoRepository LugarTrabajo { get; private set; }

        public ITipoTrabajoRepository TipoTrabajo { get; private set; }

        public IRegistroOperadoresRepository RegistroOperadores { get; private set; }


        public ITipoVehiculoRepository TipoVehiculo { get; private set; }
        public IMarcaRepository Marca { get; private set; }

        public IRepuestoRepository Repuesto { get; private set; }

        public IRepuestosMantenimientoRepository RepuestosMantenimiento { get; private set; }

        public IOperadorMantenimientoRepository OperadorMantenimiento { get; private set; }
        public object EmpresaRepository => throw new NotImplementedException();

        public UnitOfWork(ProyectoIngenieriaContext db)
        {
            _db = db;
            Empresa = new EmpresaRepository(_db);
            Vehiculo = new VehiculosRepository(_db);
            Operador = new OperadoresRepository(_db);
            CatalogoMantenimiento = new CatalogoMantenimientoRepository(_db);
            DocumentoOperador = new DocumentoOperadorRepository(_db);
            DocumentoVehiculo = new DocumentoVehiculoRepository(_db);
            HorasTrabajo = new HorasTrabajoRepository(_db);
            Notificacion = new NotificacionRepository(_db);
            RegistroCombustible = new RegistroCombustibleRepository(_db);
            RegistroMantenimiento = new RegistroMantenimientoRepository(_db);
            Proyecto = new ProyectoRepository(_db);
            LugarTrabajo = new LugarTrabajoRepository(_db);
            TipoTrabajo = new TipoTrabajoRepository(_db);
            RegistroOperadores = new RegistroOperadoresRepository(_db);
            Marca = new MarcaRepository(_db);
            TipoVehiculo = new TipoVehiculoRepository(_db);
            Repuesto = new RepuestoRepository(_db);
            RepuestosMantenimiento = new RepuestosMantenimientoRepository(_db);
            OperadorMantenimiento = new OperadorMantenimientoRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    
    }
}
