using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class RegistroOperadoresRepository : Repository<RegistroOperadore>, IRegistroOperadoresRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public RegistroOperadoresRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(RegistroOperadore registroOperadore)
        {
            _db.RegistroOperadores.Update(registroOperadore);
        }
    }
}
