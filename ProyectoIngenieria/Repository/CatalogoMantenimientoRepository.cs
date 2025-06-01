using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class CatalogoMantenimientoRepository : Repository<CatalogoMantenimiento>, ICatalogoMantenimientoRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public CatalogoMantenimientoRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CatalogoMantenimiento catalogoMantenimiento)
        {
            _db.CatalogoMantenimientos.Update(catalogoMantenimiento);
        }
    }
}
