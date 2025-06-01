using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class OperadoresRepository : Repository<Operador>, IOperadoresRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public OperadoresRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Operador operador)
        {
            _db.Operadors.Update(operador);
        }
    }
}
