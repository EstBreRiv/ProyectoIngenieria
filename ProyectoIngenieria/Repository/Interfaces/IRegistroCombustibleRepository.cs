using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IRegistroCombustibleRepository : IRepository<RegistroCombustible>
    {
        void Update(RegistroCombustible registroCombustible);
    }
}
