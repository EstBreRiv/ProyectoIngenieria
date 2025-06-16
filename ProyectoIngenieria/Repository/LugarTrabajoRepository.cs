using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class LugarTrabajoRepository : Repository<LugarTrabajo>, ILugarTrabajoRepository
    {
        private readonly ProyectoIngenieriaContext _db;
        public LugarTrabajoRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(LugarTrabajo lugarTrabajo)
        {
            _db.LugarTrabajos.Update(lugarTrabajo);
        }
    }
}
