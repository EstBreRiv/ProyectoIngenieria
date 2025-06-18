using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IRepuestoRepository : IRepository<CatalogoRepuesto>
    {
        void Update(CatalogoRepuesto repuesto);
    }
}
