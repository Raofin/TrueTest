 using OPS.Domain.Contracts;

namespace OPS.Domain;

public interface IUnitOfWork : IDisposable
{
    IExamRepository Exam { get; }
    IAccountRepository Account { get; }
    IExamCandidatesRepository ExamCandiates { get; }
    IQuestionRepository Question { get; }   

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}