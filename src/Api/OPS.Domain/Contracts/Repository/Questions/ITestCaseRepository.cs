using OPS.Domain.Contracts.Repository.Common;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts.Repository.Questions;

public interface ITestCaseRepository : IBaseRepository<TestCase>
{
    Task<List<TestCase>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken);
}