using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPS.Application.Interfaces;
using OPS.Domain.Constants;

namespace OPS.Infrastructure.Email.Configuration;

public static class FluentEmailConfig
{
    internal static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmailSender, EmailSender>();

        var settings = new EmailSettings();
        configuration.Bind(nameof(EmailSettings), settings);

        services
            .AddFluentEmail(settings.Email, ProjectConstants.ProjectName)
            .AddSmtpSender(settings.Server, settings.Port, settings.Email, settings.Password);

        return services;
    }
}