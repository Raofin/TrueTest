using Microsoft.EntityFrameworkCore;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Contracts;

namespace OPS.Persistence.Repositories;

internal class McqOptionRepository(AppDbContext dbContext) : Repository<McqOption>(dbContext), IMcqOptionRepository
{
    private readonly AppDbContext _dbContext = dbContext;
}