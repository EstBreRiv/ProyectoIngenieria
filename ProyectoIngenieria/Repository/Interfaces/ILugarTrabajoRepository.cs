using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface ILugarTrabajoRepository : IRepository<LugarTrabajo>
    {
        void Update(LugarTrabajo lugarTrabajo);
    }
}
