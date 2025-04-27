using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Application.Services.CloudService;
using OPS.Domain;

namespace OPS.Application.Features.CloudFiles.Commands;

public record EditFileCommand(Guid CloudFileId, IFormFile File) : IRequest<ErrorOr<CloudFileResponse>>;

public class EditFileCommandHandler(ICloudFileService cloudFileService, IUnitOfWork unitOfWork)
    : IRequestHandler<EditFileCommand, ErrorOr<CloudFileResponse>>
{
    private readonly ICloudFileService _cloudFileService = cloudFileService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<CloudFileResponse>> Handle(EditFileCommand request, CancellationToken cancellationToken)
    {
        var cloudFile = await _cloudFileService.UploadAsync(request.File, cancellationToken);

        if (cloudFile is null)
        {
            return Error.Failure("Failed to upload file");
        }

        _unitOfWork.CloudFile.Add(cloudFile);
        var oldFile = await _unitOfWork.CloudFile.GetAsync(request.CloudFileId, cancellationToken);

        if (oldFile is not null)
        {
            _unitOfWork.CloudFile.Remove(oldFile);
        }

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        if (result <= 0)
        {
            return Error.Failure("Failed to save file information");
        }

        _ = _cloudFileService.DeleteAsync(oldFile?.FileId);
        return cloudFile.MapToDto();
    }
}

public class EditFileCommandValidator : AbstractValidator<EditFileCommand>
{
    public EditFileCommandValidator()
    {
        RuleFor(x => x.CloudFileId)
            .NotEmpty();

        RuleFor(x => x.File)
            .NotNull()
            .Must(file => file.Length > 0);
    }
}