using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IOperadorMantenimientoRepository : IRepository<OperadorMantenimiento>
    {
        void Update(OperadorMantenimiento operadorMantenimiento);
    }
}
