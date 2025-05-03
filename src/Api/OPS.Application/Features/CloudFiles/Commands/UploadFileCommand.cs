using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using OPS.Application.Dtos;
using OPS.Application.Interfaces.Auth;
using OPS.Application.Interfaces.Cloud;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.CloudFiles.Commands;

public record UploadFileCommand(IFormFile File) : IRequest<ErrorOr<CloudFileResponse>>;

public class UploadFileCommandHandler(
    ICloudFileService cloudFileService,
    IUserProvider userProvider,
    IUnitOfWork unitOfWork) : IRequestHandler<UploadFileCommand, ErrorOr<CloudFileResponse>>
{
    private readonly ICloudFileService _cloudFileService = cloudFileService;
    private readonly IUserProvider _userProvider = userProvider;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<CloudFileResponse>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var cloudFile = await _cloudFileService.UploadAsync(request.File, cancellationToken);

        if (cloudFile is null)
        {
            return Error.Failure("Failed to upload file");
        }

        cloudFile.AccountId = _userProvider.TryGetAccountId();
        _unitOfWork.CloudFile.Add(cloudFile);

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? cloudFile.MapToDto()
            : Error.Failure("Failed to save file information");
    }
}

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .Must(file => file.Length > 0)
            .WithMessage("No file uploaded.")
            .Must(file => file.Length <= 102400) // 100 KB limit
            .WithMessage("File size exceeds the 100 KB limit.");
    }
}