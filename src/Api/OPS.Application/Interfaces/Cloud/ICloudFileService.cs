using Microsoft.AspNetCore.Http;
using OPS.Application.Dtos;
using OPS.Domain.Entities.Core;

namespace OPS.Application.Interfaces.Cloud;

public interface ICloudFileService
{
    Task<CloudFile?> UploadAsync(IFormFile formFile, CancellationToken cancellationToken = default);
    Task<FileDownloadResponse?> DownloadAsync(string fileId);
    Task DeleteAsync(string? fileId);
}