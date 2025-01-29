using Microsoft.EntityFrameworkCore;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Interfaces.Repositories;

namespace OPS.Persistence.Repositories;

internal class ExamRepository(AppDbContext dbContext) : Repository<Examination>(dbContext), IExamRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<Examination>> GetUpcomingExamsAsync()
    {
        return await _dbContext.Exams
            .Where(exam => exam.OpensAt > DateTime.UtcNow)
            .OrderBy(exam => exam.OpensAt)
            .ToListAsync();
    }
}