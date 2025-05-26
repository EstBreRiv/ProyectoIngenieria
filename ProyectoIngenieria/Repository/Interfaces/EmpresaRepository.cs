namespace ProyectoIngenieria.Repository.Interfaces
{
    public class EmpresaRepository : Repository<Models.Empresa>, IEmpresaRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public EmpresaRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void update(Models.Empresa empresa)
        {
            _db.Empresas.Update(empresa);
        }
    
    }
}
