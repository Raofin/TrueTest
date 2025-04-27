using OPS.Application.Dtos;
using OPS.Domain.Contracts.Core.GoogleCloud;
using OPS.Domain.Entities.Core;

namespace OPS.Application.Mappers;

public static class FileMappers
{
    public static CloudFile? MapToCloudFile(this GoogleFile? googleFile, Guid? accountId = null)
    {
        return googleFile is null
            ? null
            : new CloudFile
            {
                FileId = googleFile.CloudFileId,
                Name = googleFile.Name,
                ContentType = googleFile.ContentType,
                Size = googleFile.Size,
                AccountId = accountId
            };
    }

    public static CloudFileResponse MapToDto(this CloudFile imageFile)
    {
        return new CloudFileResponse(
            imageFile.Id,
            imageFile.FileId,
            imageFile.Name,
            imageFile.ContentType,
            imageFile.Size,
            imageFile.FileId.ToWebContentLink(),
            imageFile.FileId.ToWebViewLink(),
            imageFile.FileId.ToDirectLink(),
            imageFile.CreatedAt
        );
    }

    public static FileDownloadResponse MapToDto(this GoogleFileDownload googleFile)
    {
        return new FileDownloadResponse(
            googleFile.Name,
            googleFile.ContentType,
            googleFile.Size,
            googleFile.Bytes
        );
    }

    private static string ToWebContentLink(this string fileId) =>
        $"https://drive.google.com/uc?id={fileId}&export=download";

    private static string ToWebViewLink(this string fileId) =>
        $"https://drive.google.com/file/d/{fileId}/view?usp=drivesdk";

    private static string ToDirectLink(this string fileId) =>
        $"https://lh3.googleusercontent.com/d/{fileId}";
}