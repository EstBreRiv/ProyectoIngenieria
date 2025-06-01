using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class HorasTrabajoRepository : Repository<HorasTrabajo>, IHorasTrabajoRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public HorasTrabajoRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(HorasTrabajo horasTrabajo)
        {
            _db.HorasTrabajos.Update(horasTrabajo);
        }
    }
}
