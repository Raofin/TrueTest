using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Upload;
using Microsoft.Extensions.Caching.Memory;
using OPS.Domain.Contracts.Core.GoogleCloud;
using Serilog;

namespace OPS.Infrastructure.GoogleCloud;

using File = Google.Apis.Drive.v3.Data.File;

internal class GoogleCloudService(IMemoryCache cache, DriveService driveService) : IGoogleCloudService
{
    private readonly IMemoryCache _cache = cache;
    private readonly DriveService _driveService = driveService;

    public async Task<GoogleFile?> UploadAsync(Stream stream, string fileName, string contentType)
    {
        try
        {
            var metaData = CreateMetaData(fileName);
            var request = _driveService.Files.Create(metaData, stream, contentType);

            request.Fields = "id, name, mimeType, size, webContentLink, webViewLink, createdTime";
            var result = await request.UploadAsync();

            if (result.Status == UploadStatus.Failed)
            {
                Log.Error("Error uploading file: {FileName}, Error: {Error}", fileName, result.Exception);
                return null;
            }

            SetPermissions(request.ResponseBody.Id);

            return MapToGoogleFile(request.ResponseBody);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error uploading file: {FileName}", fileName);
            return null;
        }
    }

    private void SetPermissions(string fileId)
    {
        var permission = new Permission
        {
            Type = "anyone",
            Role = "reader"
        };

        _driveService.Permissions
            .Create(permission, fileId)
            .ExecuteAsync();
    }

    public async Task<GoogleFile?> InfoAsync(string fileId)
    {
        try
        {
            var request = _driveService.Files.Get(fileId);
            request.Fields = "id, name, mimeType, size, webContentLink, webViewLink, createdTime";
            var response = await request.ExecuteAsync();
            return MapToGoogleFile(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving file info for file with ID: {FileId}", fileId);
            return null;
        }
    }

    public async Task<GoogleFileDownload?> DownloadAsync(string fileId)
    {
        try
        {
            var request = _driveService.Files.Get(fileId);
            request.Fields = "id, name, mimeType, size, createdTime";
            var fileInfo = await request.ExecuteAsync();

            using var stream = new MemoryStream();
            await request.DownloadAsync(stream);

            return new GoogleFileDownload(
                fileInfo.Id,
                fileInfo.Name,
                fileInfo.MimeType,
                fileInfo.Size ?? 0,
                stream.ToArray(),
                fileInfo.CreatedTimeDateTimeOffset?.UtcDateTime ?? DateTime.UtcNow
            );
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error downloading file with ID: {FileId}", fileId);
            return null;
        }
    }

    public async Task DeleteAsync(string fileId)
    {
        try
        {
            await _driveService.Files.Delete(fileId).ExecuteAsync();

        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting file with ID: {FileId}", fileId);
        }
    }

    private File CreateMetaData(string fileName)
    {
        const string cacheKey = "TT_FolderId";
        var metaData = new File { Name = fileName, };

        if (_cache.TryGetValue(cacheKey, out string? fileId) && fileId is not null)
        {
            metaData.Parents = new List<string> { fileId };
        }

        return metaData;
    }

    private static GoogleFile MapToGoogleFile(File file)
    {
        return new GoogleFile(
            file.Id,
            file.Name,
            file.MimeType,
            file.Size ?? 0,
            file.CreatedTimeDateTimeOffset?.UtcDateTime ?? DateTime.Now
        );
    }
}