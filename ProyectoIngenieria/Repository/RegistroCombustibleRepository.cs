using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class RegistroCombustibleRepository : Repository<RegistroCombustible>, IRegistroCombustibleRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public RegistroCombustibleRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(RegistroCombustible registroCombustible)
        {
            _db.RegistroCombustibles.Update(registroCombustible);
        }
    }
}
