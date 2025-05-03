using OPS.Domain.Entities.Exam;
using OPS.Domain.Interfaces.Common;

namespace OPS.Domain.Interfaces.Questions;

public interface ITestCaseRepository : IBaseRepository<TestCase>
{
    Task<List<TestCase>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken);
}