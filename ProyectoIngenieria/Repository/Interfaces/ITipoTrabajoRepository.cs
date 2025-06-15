using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface ITipoTrabajoRepository : IRepository<TipoTrabajo>
    {
        void Update(TipoTrabajo tipoTrabajo);
    }
}
