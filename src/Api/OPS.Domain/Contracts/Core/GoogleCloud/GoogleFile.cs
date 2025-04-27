namespace OPS.Domain.Contracts.Core.GoogleCloud;

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