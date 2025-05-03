using OPS.Domain.Entities.Submit;
using OPS.Domain.Interfaces.Submissions;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Submissions;

internal class TestCaseOutputRepository(AppDbContext dbContext)
    : Repository<TestCaseOutput>(dbContext), ITestCaseOutputRepository;