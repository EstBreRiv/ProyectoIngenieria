namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IEmpresaRepository : IRepository<Models.Empresa>
    {
        void update(Models.Empresa empresa);
    }
    
}
