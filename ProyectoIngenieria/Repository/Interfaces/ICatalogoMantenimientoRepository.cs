using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface ICatalogoMantenimientoRepository : IRepository<CatalogoMantenimiento>
    {
        void Update(CatalogoMantenimiento catalogoMantenimiento);
    }
}
