using Microsoft.Extensions.DependencyInjection;
using OPS.Domain;
using OPS.Domain.Contracts;
using OPS.Persistence.Repositories;

namespace OPS.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IExamRepository, ExamRepository>();

        services.AddScoped<IAccountRepository, AccountRepository>();

        services.AddScoped<IExamCandidatesRepository, ExamCandidatesRepository>();

        services.AddScoped<IQuestionRepository, QuestionRepository>();

        services.AddScoped<IWrittenSubmissionRepository, WrittenSubmissionRepository>();

        return services;
    }
}
