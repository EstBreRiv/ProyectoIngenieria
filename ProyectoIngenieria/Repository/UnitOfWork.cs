using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProyectoIngenieriaContext _db;

        public IEmpresaRepository Empresa { get; private set; }
        public IVehiculosRepository Vehiculo { get; private set; }

        public object EmpresaRepository => throw new NotImplementedException();

        public UnitOfWork(ProyectoIngenieriaContext db)
        {
            _db = db;
            Empresa = new EmpresaRepository(_db);
            Vehiculo = new VehiculosRepository(_db);

        }

        public void Save()
        {
            _db.SaveChanges();
        }
    
    }
}
