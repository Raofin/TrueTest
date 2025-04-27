namespace OPS.Application.Dtos;

public record CloudFileResponse(
    Guid CloudFileId,
    string FileId,
    string Name,
    string ContentType,
    long Size,
    string WebContentLink,
    string WebViewLink,
    string DirectLink,
    DateTime CreatedAt
);

public record FileDownloadResponse(
    string FileName,
    string ContentType,
    long Size,
    byte[] Bytes
);