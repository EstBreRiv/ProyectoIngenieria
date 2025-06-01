using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IHorasTrabajoRepository : IRepository<HorasTrabajo>
    {
        void Update(HorasTrabajo horasTrabajo);
    }
}
