using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class OperadorMantenimientoRepository : Repository<OperadorMantenimiento>, IOperadorMantenimientoRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public OperadorMantenimientoRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OperadorMantenimiento operadorMantenimiento)
        {
            _db.OperadorMantenimientos.Update(operadorMantenimiento);
        }
    }
}
