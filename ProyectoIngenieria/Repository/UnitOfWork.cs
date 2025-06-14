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
        public IRegistroOperadoresRepository RegistroOperador { get; private set; }

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
            RegistroOperador = new RegistroOperadoresRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    
    }
}
