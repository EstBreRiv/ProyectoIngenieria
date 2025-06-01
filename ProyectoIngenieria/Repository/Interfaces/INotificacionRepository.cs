using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface INotificacionRepository : IRepository<Notificacion>
    {
        void Update(Notificacion notificacion);
    }
}
