using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Services.CloudService;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;

namespace OPS.Application.Features.CloudFiles.Commands;

public record DeleteFileCommand(Guid CloudFileId) : IRequest<ErrorOr<Success>>;

public class DeleteFileCommandHandler(
    ICloudFileService cloudFileService,
    IUserInfoProvider userInfoProvider,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteFileCommand, ErrorOr<Success>>
{
    private readonly ICloudFileService _cloudFileService = cloudFileService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var cloudFile = await _unitOfWork.CloudFile.GetAsync(request.CloudFileId, cancellationToken);

        if (cloudFile is null) return Error.NotFound();

        _unitOfWork.CloudFile.Remove(cloudFile);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

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