using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPS.Application.Common.Constants;
using OPS.Domain.Contracts.Core.EmailSender;
using OPS.Infrastructure.EmailSender;

namespace OPS.Infrastructure.AppConfiguration.Email;

public static class EmailServices
{
    internal static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccountEmails, AccountEmails>();

        var settings = new EmailSettings();
        configuration.Bind(nameof(EmailSettings), settings);

        services
            .AddFluentEmail(settings.Email, ProjectConstants.ProjectName)
            .AddSmtpSender(settings.Server, settings.Port, settings.Email, settings.Password);

        return services;
    }
}