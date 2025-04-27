namespace OPS.Domain.Contracts.Core.GoogleCloud;

public interface IGoogleCloudService
{
    Task<GoogleFile?> UploadAsync(Stream stream, string fileName, string contentType);
    Task<GoogleFile?> InfoAsync(string fileId);
    Task<GoogleFileDownload?> DownloadAsync(string fileId);
    Task DeleteAsync(string fileId);
}