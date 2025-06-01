using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class NotificacionRepository : Repository<Notificacion>, INotificacionRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public NotificacionRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Notificacion notificacion)
        {
            _db.Notificacions.Update(notificacion);
        }
    }
}
