using OPS.Domain.Contracts;
using OPS.Domain.Contracts.Repository;

namespace OPS.Domain;

public interface IUnitOfWork : IDisposable
{
    IAccountRepository Account { get; }
    IOtpRepository Otp { get; }
    IExamRepository Exam { get; }
    IExamCandidatesRepository ExamCandidate { get; }
    IQuestionRepository Question { get; }
    IWrittenSubmissionRepository WrittenSubmission { get; }
    IMcqSubmissionRepository McqSubmission { get; }
    IMcqOptionRepository McqOption { get; } 
    IProfileRepository Profile { get; }
    IProfileLinkRepository ProfileLink { get; }
    IProblemSubmissionRepository ProblemSubmission { get; }
    ISubmissionRepository Submission { get; }
    ITestCaseOutputRepository TestCaseOutput { get; }
    ICloudFileRepository CloudFile { get; }
    ITestCaseRepository TestCase { get; }
    IAdminInviteRepository AdminInvite { get; }

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}