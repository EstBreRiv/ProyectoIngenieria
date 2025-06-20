using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class RepuestosMantenimientoRepository : Repository<RepuestosMantenimiento>, IRepuestosMantenimientoRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public RepuestosMantenimientoRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(RepuestosMantenimiento repuestosMantenimiento)
        {
            _db.RepuestosMantenimientos.Update(repuestosMantenimiento);
        }
    }
}
