namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IVehiculosRepository : IRepository<Models.Vehiculo>
    {
        void Update(Models.Vehiculo vehiculo);
    }
}
