using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IProyectoRepository : IRepository<Proyecto>
    {
        void Update(Proyecto proyecto);
    }
}
