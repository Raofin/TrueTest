using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.User.Commands;

public record DeleteProfileLinkCommand(Guid ProfileLinkId) : IRequest<ErrorOr<Success>>;

public class DeleteProfileLinkCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProfileLinkCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteProfileLinkCommand request, CancellationToken cancellationToken)
    {
        var profileLink = await _unitOfWork.ProfileLink.GetAsync(request.ProfileLinkId, cancellationToken);

        if (profileLink is null) return Error.NotFound();

        _unitOfWork.ProfileLink.Remove(profileLink);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0 ? Result.Success : Error.Unexpected();
    }
}

public class DeleteProfileLinkCommandValidator : AbstractValidator<DeleteProfileLinkCommand>
{
    public DeleteProfileLinkCommandValidator()
    {
        RuleFor(x => x.ProfileLinkId)
            .IsValidGuid();
    }
}