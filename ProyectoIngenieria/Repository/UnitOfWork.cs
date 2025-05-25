using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProyectoIngenieriaContext _db;

        public IEmpresaRepository Empresa { get; private set; }

        public UnitOfWork(ProyectoIngenieriaContext db)
        {
            _db = db;
            Empresa = new EmpresaRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    
    }
}
