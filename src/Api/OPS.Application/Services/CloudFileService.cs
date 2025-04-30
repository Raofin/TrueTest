using Microsoft.AspNetCore.Http;
using OPS.Application.Dtos;
using OPS.Application.Interfaces.Cloud;
using OPS.Application.Mappers;
using OPS.Domain.Entities.Core;

namespace OPS.Application.Services;

/// <summary>
/// Implementation of the <see cref="ICloudFileService"/> interface for managing cloud file operations.
/// </summary>
internal class CloudFileService(IGoogleCloudService googleCloudService) : ICloudFileService
{
    private readonly IGoogleCloudService _googleCloudService = googleCloudService;

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<FileDownloadResponse?> DownloadAsync(string fileId)
    {
        var file = await _googleCloudService.DownloadAsync(fileId);
        return file?.MapToDto();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(string? fileId)
    {
        if (fileId is not null)
        {
            await _googleCloudService.DeleteAsync(fileId);
        }
    }
}