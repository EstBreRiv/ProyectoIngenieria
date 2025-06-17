using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class TipoTrabajoRepository : Repository<TipoTrabajo>, ITipoTrabajoRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public TipoTrabajoRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db; 
        }

        public void Update(TipoTrabajo tipoTrabajo)
        {
            _db.TipoTrabajos.Update(tipoTrabajo);
        }

    }
}
