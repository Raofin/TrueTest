using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPS.Application.Interfaces;
using OPS.Domain.Constants;

namespace OPS.Infrastructure.Email.Configuration;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to add email related services using FluentEmail.
/// </summary>
public static class FluentEmailConfig
{
    /// <summary>
    /// Adds email sending services to the service collection using FluentEmail.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing application settings, including email configuration.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
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