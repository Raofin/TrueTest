using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Serilog;

namespace OPS.Infrastructure.Cloud.Configuration;

/// <summary>
/// Provides a Google Drive service client.
/// </summary>
public class DriveServiceProvider
{
    /// <summary>
    /// Gets the ID of the Google Drive folder used by the application.
    /// </summary>
    public static string FolderId { get; private set; } = null!;
    private readonly GoogleCloudSettings _settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="DriveServiceProvider"/> class.
    /// </summary>
    /// <param name="settings">The <see cref="GoogleCloudSettings"/> containing Google Cloud configuration.</param>
    public DriveServiceProvider(GoogleCloudSettings settings)
    {
        _settings = settings;
        FolderId = settings.FolderId;
    }

    /// <summary>
    /// Creates and returns a new instance of the Google Drive service client.
    /// </summary>
    /// <returns>A <see cref="DriveService"/> client, or a new empty <see cref="DriveService"/> if credentials are invalid.</returns>
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