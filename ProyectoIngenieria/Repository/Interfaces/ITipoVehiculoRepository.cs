using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface ITipoVehiculoRepository : IRepository<TipoVehiculo>
    {
        void Update(TipoVehiculo tipoVehiculo);
    }
    
}
