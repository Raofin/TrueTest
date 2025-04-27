using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Application.Services.CloudService;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.Core;

namespace OPS.Application.Features.CloudFiles.Commands;

public record UploadFileCommand(IFormFile File) : IRequest<ErrorOr<CloudFileResponse>>;

public class UploadFileCommandHandler(
    ICloudFileService cloudFileService,
    IUserInfoProvider userInfoProvider,
    IUnitOfWork unitOfWork) : IRequestHandler<UploadFileCommand, ErrorOr<CloudFileResponse>>
{
    private readonly ICloudFileService _cloudFileService = cloudFileService;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<CloudFileResponse>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        CloudFile? cloudFile = await _cloudFileService.UploadAsync(request.File, cancellationToken);

        if (cloudFile is null)
        {
            return Error.Failure("Failed to upload file");
        }

        cloudFile.AccountId = _userInfoProvider.TryGetAccountId();
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
            .Must(file => file.Length > 0);
    }
}