using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class RegistroMantenimientoRepository : Repository<RegistroMantenimiento>, IRegistroMantenimientoRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public RegistroMantenimientoRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(RegistroMantenimiento registroMantenimiento)
        {
            _db.RegistroMantenimientos.Update(registroMantenimiento);
        }
    }
}
