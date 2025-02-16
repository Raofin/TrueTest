using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OPS.Domain;

namespace OPS.Application.BackgroundServices;

internal class OtpCleanupService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromDays(1), cancellationToken);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var otps = await unitOfWork.Otp.GetExpiredOtpsAsync(cancellationToken);

                unitOfWork.Otp.RemoveRange(otps);

                await unitOfWork.CommitAsync(cancellationToken);
            }
        }
    }
}