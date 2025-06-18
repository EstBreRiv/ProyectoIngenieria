using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IMarcaRepository : IRepository<Marca>
    {
        void Update(Marca marca);
    }
}
