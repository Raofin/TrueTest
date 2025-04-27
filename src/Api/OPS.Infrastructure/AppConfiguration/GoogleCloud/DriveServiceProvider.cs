using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Serilog;

namespace OPS.Infrastructure.AppConfiguration.GoogleCloud;

public class DriveServiceProvider
{
    public static string FolderId { get; private set; } = null!;
    private readonly GoogleCloudSettings _settings;

    public DriveServiceProvider(GoogleCloudSettings settings)
    {
        _settings = settings;
        FolderId = settings.FolderId;
    }

    public DriveService GetDriveService()
    {
        if (!File.Exists(_settings.Credentials) || File.ReadAllLines(_settings.Credentials).Length < 10)
        {
            Log.Error("Google credentials file not found or invalid.");
            return new DriveService();
        }

        var credential = GoogleCredential.FromFile(_settings.Credentials)
            .CreateScoped(DriveService.Scope.Drive);

        var driveService = new DriveService(
            new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "TrueTest",
            }
        );

        return driveService;
    }
}