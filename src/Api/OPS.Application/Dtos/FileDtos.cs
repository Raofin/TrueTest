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

public record GoogleFile(
    string CloudFileId,
    string Name,
    string ContentType,
    long Size,
    DateTime CreatedAt
);

public record GoogleFileDownload(
    string CloudFileId,
    string Name,
    string ContentType,
    long Size,
    byte[] Bytes,
    DateTime CreatedAt
);