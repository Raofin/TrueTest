using OPS.Domain.Contracts.Repository.Core;
using OPS.Domain.Contracts.Repository.Exams;
using OPS.Domain.Contracts.Repository.Questions;
using OPS.Domain.Contracts.Repository.Submissions;
using OPS.Domain.Contracts.Repository.Users;

namespace OPS.Domain;

public interface IUnitOfWork : IDisposable
{
    IAccountRepository Account { get; }
    IAccountRoleRepository AccountRole { get; }
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
    ITestCaseOutputRepository TestCaseOutput { get; }
    ICloudFileRepository CloudFile { get; }
    ITestCaseRepository TestCase { get; }
    IAdminInviteRepository AdminInvite { get; }

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}