using ProyectoIngenieria.Models;
using ProyectoIngenieria.Repository.Interfaces;

namespace ProyectoIngenieria.Repository
{
    public class RepuestoRepository : Repository<CatalogoRepuesto>, IRepuestoRepository
    {
        private readonly ProyectoIngenieriaContext _context;

        public RepuestoRepository(ProyectoIngenieriaContext context) : base(context)
        {
            _context = context;
        }

        public void Update(CatalogoRepuesto repuesto)
        {
            var entidadDb = _context.CatalogoRepuestos.FirstOrDefault(r => r.Id == repuesto.Id);
            if (entidadDb != null)
            {
                entidadDb.Nombre = repuesto.Nombre;
                entidadDb.Descripcion = repuesto.Descripcion;
                entidadDb.PrecioEstimado = repuesto.PrecioEstimado;
            }
        }
    }
}