using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IOperadoresRepository : IRepository<Operador>
    {
        void Update(Operador operador);
    }
}
