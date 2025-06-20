using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IRepuestosMantenimientoRepository : IRepository<RepuestosMantenimiento>
    {
        void Update(RepuestosMantenimiento repuestosMantenimiento);
    }
}
