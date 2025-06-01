using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IDocumentoVehiculoRepository : IRepository<DocumentoVehiculo>
    {
        void Update(DocumentoVehiculo documentoVehiculo);
    }
}
