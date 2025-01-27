namespace OPS.Domain;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync();
}