using OPS.Application.Dtos;

namespace OPS.Application.Interfaces.Cloud;

public interface IGoogleCloudService
{
    Task<GoogleFile?> UploadAsync(Stream stream, string fileName, string contentType);
    Task<GoogleFile?> InfoAsync(string fileId);
    Task<GoogleFileDownload?> DownloadAsync(string fileId);
    Task DeleteAsync(string fileId);
}