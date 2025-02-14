using OPS.Domain.Contracts.Repository;

namespace OPS.Domain;

public interface IUnitOfWork : IDisposable
{
    IExamRepository Exam { get; }
    IAccountRepository Account { get; }

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}