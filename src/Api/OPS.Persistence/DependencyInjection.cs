using Microsoft.Extensions.DependencyInjection;
using OPS.Domain;
using OPS.Domain.Interfaces.Core;
using OPS.Domain.Interfaces.Exams;
using OPS.Domain.Interfaces.Questions;
using OPS.Domain.Interfaces.Submissions;
using OPS.Domain.Interfaces.Users;
using OPS.Persistence.Repositories.Cores;
using OPS.Persistence.Repositories.Exams;
using OPS.Persistence.Repositories.Questions;
using OPS.Persistence.Repositories.Submissions;
using OPS.Persistence.Repositories.Users;

namespace OPS.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountRoleRepository, AccountRoleRepository>();
        services.AddScoped<IOtpRepository, OtpRepository>();
        services.AddScoped<IExamRepository, ExamRepository>();
        services.AddScoped<IExamCandidatesRepository, ExamCandidatesRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IWrittenSubmissionRepository, WrittenSubmissionRepository>();
        services.AddScoped<IMcqSubmissionRepository, McqSubmissionRepository>();
        services.AddScoped<IMcqOptionRepository, McqOptionRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IProfileLinkRepository, ProfileLinkRepository>();
        services.AddScoped<IProblemSubmissionRepository, ProblemSubmissionRepository>();
        services.AddScoped<ITestCaseOutputRepository, TestCaseOutputRepository>();
        services.AddScoped<ICloudFileRepository, CloudFIleRepository>();
        services.AddScoped<ITestCaseRepository, TestCaseRepository>();
        services.AddScoped<IAdminInviteRepository, AdminInviteRepository>();

        return services;
    }
}