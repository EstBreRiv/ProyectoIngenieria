using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class ProyectoRepository : Repository<Proyecto>, IProyectoRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public ProyectoRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Proyecto proyecto)
        {
            _db.Proyectos.Update(proyecto);
        }
    }
}
