using OPS.Domain.Entities.Exam;
using OPS.Domain.Interfaces.Questions;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Questions;

internal class McqOptionRepository(AppDbContext dbContext) : Repository<McqOption>(dbContext), IMcqOptionRepository;