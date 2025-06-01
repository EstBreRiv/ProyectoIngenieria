using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class DocumentoVehiculoRepository : Repository<DocumentoVehiculo>, IDocumentoVehiculoRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public DocumentoVehiculoRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(DocumentoVehiculo documentoVehiculo)
        {
            _db.DocumentoVehiculos.Update(documentoVehiculo);
        }
    }
}
