using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class RegistroOperadoresRepository : Repository<RegistroOperadores>, IRegistroOperadoresRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public RegistroOperadoresRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(RegistroOperadores registroOperadores)
        {
            _db.RegistroOperadores.Update(registroOperadores);
        }
    }
}
