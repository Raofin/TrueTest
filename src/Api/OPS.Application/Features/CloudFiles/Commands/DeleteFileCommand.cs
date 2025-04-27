using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Services.CloudService;
using OPS.Domain;

namespace OPS.Application.Features.CloudFiles.Commands;

public record DeleteFileCommand(Guid CloudFileId) : IRequest<ErrorOr<Success>>;

public class DeleteFileCommandHandler(ICloudFileService cloudFileService, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteFileCommand, ErrorOr<Success>>
{
    private readonly ICloudFileService _cloudFileService = cloudFileService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var cloudFile = await _unitOfWork.CloudFile.GetAsync(request.CloudFileId, cancellationToken);

        if (cloudFile is null) return Error.NotFound();

        _unitOfWork.CloudFile.Remove(cloudFile);
        await _unitOfWork.CommitAsync(cancellationToken);

        _ = _cloudFileService.DeleteAsync(cloudFile.FileId);
        return Result.Success;
    }
}

public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
{
    public DeleteFileCommandValidator()
    {
        RuleFor(x => x.CloudFileId).NotEmpty();
    }
}