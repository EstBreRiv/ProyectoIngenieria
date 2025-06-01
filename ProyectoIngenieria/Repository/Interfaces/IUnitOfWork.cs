namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IEmpresaRepository Empresa { get; }
        IVehiculosRepository Vehiculo { get; }
        object EmpresaRepository { get; }

        void Save();
    }
}
