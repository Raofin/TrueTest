using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Services.CloudService;

namespace OPS.Application.Features.CloudFiles.Queries;

public record FileDownloadCommand(string FileId) : IRequest<ErrorOr<FileDownloadResponse>>;

public class FileDownloadCommandHandler(ICloudFileService cloudFileService)
    : IRequestHandler<FileDownloadCommand, ErrorOr<FileDownloadResponse>>
{
    private readonly ICloudFileService _cloudFileService = cloudFileService;

    public async Task<ErrorOr<FileDownloadResponse>> Handle(FileDownloadCommand request,
        CancellationToken cancellationToken)
    {
        var file = await _cloudFileService.DownloadAsync(request.FileId);

        return file is null
            ? Error.NotFound()
            : file;
    }
}

public class FileDownloadCommandValidator : AbstractValidator<FileDownloadCommand>
{
    public FileDownloadCommandValidator()
    {
        RuleFor(x => x.FileId).NotEmpty();
    }
}