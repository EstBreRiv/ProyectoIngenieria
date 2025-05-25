namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IEmpresaRepository Empresa { get; }

        void Save();
    }
}
