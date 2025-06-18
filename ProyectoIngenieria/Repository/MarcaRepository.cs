using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class MarcaRepository : Repository<Marca>, IMarcaRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public MarcaRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Marca marca)
        {
            _db.Marcas.Update(marca);
        }
        
    }
}
