using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class TipoVehiculoRepository : Repository<TipoVehiculo>, ITipoVehiculoRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public TipoVehiculoRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TipoVehiculo tipoVehiculo)
        {
            _db.TipoVehiculos.Update(tipoVehiculo);
        }

    }
   
}
