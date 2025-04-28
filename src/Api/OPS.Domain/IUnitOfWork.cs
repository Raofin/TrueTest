using OPS.Domain.Interfaces.Core;
using OPS.Domain.Interfaces.Exams;
using OPS.Domain.Interfaces.Questions;
using OPS.Domain.Interfaces.Submissions;
using OPS.Domain.Interfaces.Users;

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