using ProyectoIngenieria.Models;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IDocumentoOperadorRepository : IRepository<DocumentoOperador>
    {
        void Update(DocumentoOperador DocumentoOperador);
    }
}
