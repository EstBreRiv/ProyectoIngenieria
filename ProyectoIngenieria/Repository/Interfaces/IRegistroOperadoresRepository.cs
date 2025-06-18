using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IRegistroOperadoresRepository : IRepository<RegistroOperadore>
    {
        void Update(RegistroOperadore registroOperadores);
    }
}
