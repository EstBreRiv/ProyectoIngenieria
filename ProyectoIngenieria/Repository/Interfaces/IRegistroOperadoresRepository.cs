using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IRegistroOperadoresRepository : IRepository<RegistroOperadores>
    {
        void Update(RegistroOperadores registroOperadores);
    }
}
