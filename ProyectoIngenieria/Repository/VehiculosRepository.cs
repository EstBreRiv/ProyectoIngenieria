using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class VehiculosRepository : Repository<Models.Vehiculo>, IVehiculosRepository
    {
        private readonly ProyectoIngenieriaContext _db;

        public VehiculosRepository(ProyectoIngenieriaContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Models.Vehiculo vehiculo)
        {
           _db.Vehiculos.Update(vehiculo);
        }
    

    }
}
