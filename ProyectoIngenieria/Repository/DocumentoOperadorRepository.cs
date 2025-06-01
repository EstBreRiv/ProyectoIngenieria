using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class DocumentoOperadorRepository : Repository<DocumentoOperador>, IDocumentoOperadorRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public DocumentoOperadorRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(DocumentoOperador documentoOperador)
        {
            _db.DocumentoOperadors.Update(documentoOperador);
        }
    }
}
