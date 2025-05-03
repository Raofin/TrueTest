using OPS.Domain.Interfaces.Core;
using OPS.Domain.Interfaces.Exams;
using OPS.Domain.Interfaces.Questions;
using OPS.Domain.Interfaces.Submissions;
using OPS.Domain.Interfaces.Users;

namespace OPS.Domain;

/// <summary>
/// Defines the contract for a Unit of Work, which serves as a single point of access to all data repositories
/// and provides a mechanism for atomic database transactions.
/// </summary>
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

    /// <summary>
    /// Asynchronously saves all changes made to the context to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the underlying database.</returns>
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}