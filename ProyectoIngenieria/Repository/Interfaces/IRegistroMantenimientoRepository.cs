using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IRegistroMantenimientoRepository : IRepository<RegistroMantenimiento>
    {
        void Update(RegistroMantenimiento registroMantenimiento);
    }
}
