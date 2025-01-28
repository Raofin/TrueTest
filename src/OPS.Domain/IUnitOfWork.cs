using OPS.Domain.Interfaces.Repositories;

namespace OPS.Domain;

public interface IUnitOfWork : IDisposable
{
    IExamRepository Exam { get; }

    Task<int> CommitAsync();
}