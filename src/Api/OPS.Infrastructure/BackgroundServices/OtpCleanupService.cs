using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OPS.Domain;

namespace OPS.Infrastructure.BackgroundServices;

[ExcludeFromCodeCoverage]
internal class OtpCleanupService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    /// <summary>
    /// Executes the background service, periodically cleaning up expired One-Time Passwords (OTPs) from the database.
    /// </summary>
    /// <param name="stoppingToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var otps = await unitOfWork.Otp.GetExpiredOtpsAsync(stoppingToken);

                unitOfWork.Otp.RemoveRange(otps);

                await unitOfWork.CommitAsync(stoppingToken);
            }
        }
    }
}