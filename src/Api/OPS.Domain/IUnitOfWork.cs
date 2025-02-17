 using OPS.Domain.Contracts;

namespace OPS.Domain;

public interface IUnitOfWork : IDisposable
{
    IAccountRepository Account { get; }
    IExamCandidatesRepository ExamCandiates { get; }
    IQuestionRepository Question { get; }   

    IWrittenSubmissionRepository WrittenSubmission { get; }     
    IOtpRepository Otp { get; }
    IExamRepository Exam { get; }

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}