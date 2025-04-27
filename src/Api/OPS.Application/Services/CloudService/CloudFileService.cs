using Microsoft.AspNetCore.Http;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain.Contracts.Core.GoogleCloud;
using OPS.Domain.Entities.Core;

namespace OPS.Application.Services.CloudService;

internal class CloudFileService(IGoogleCloudService googleCloudService) : ICloudFileService
{
    private readonly IGoogleCloudService _googleCloudService = googleCloudService;

    public async Task<CloudFile?> UploadAsync(IFormFile formFile, CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream();
        await formFile.CopyToAsync(stream, cancellationToken);

        var uploadedFile = await _googleCloudService.UploadAsync(
            stream,
            Path.GetFileName(formFile.FileName),
            formFile.ContentType
        );

        return uploadedFile.MapToCloudFile();
    }

    public async Task<FileDownloadResponse?> DownloadAsync(string fileId)
    {
        var file = await _googleCloudService.DownloadAsync(fileId);
        return file?.MapToDto();
    }

    public async Task DeleteAsync(string? fileId)
    {
        if (fileId is not null)
        {
            await _googleCloudService.DeleteAsync(fileId);
        }
    }
}