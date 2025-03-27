using OPS.Domain.Contracts.Repository.Common;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.User;

namespace OPS.Domain.Contracts.Repository.Exams;

public interface IExamRepository : IBaseRepository<Examination>
{
    Task<List<Examination>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    Task<Examination?> GetWithQuestionsAsync(Guid examId, CancellationToken cancellationToken);
    Task<Account> GetWithAllQuesAndSubmission(Guid examId, Guid accountId, CancellationToken cancellationToken);
}