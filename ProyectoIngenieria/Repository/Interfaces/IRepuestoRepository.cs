using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IRepuestoRepository : IRepository<Repuesto>
    {
        void Update(Repuesto repuesto);
    }
}
