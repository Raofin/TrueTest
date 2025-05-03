using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.Accounts.Commands;

public record ChangeActiveStatusCommand(Guid AccountId) : IRequest<ErrorOr<AccountResponse>>;

public class ChangeActiveStatusCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeActiveStatusCommand, ErrorOr<AccountResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<AccountResponse>> Handle(
        ChangeActiveStatusCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Account.GetAsync(request.AccountId, cancellationToken);
        if (account is null) return Error.NotFound();

        account.IsActive = !account.IsActive;

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? account.MapToDto()
            : Error.Unexpected();
    }
}

public class ChangeActiveStatusValidator : AbstractValidator<ChangeActiveStatusCommand>
{
    public ChangeActiveStatusValidator()
    {
        RuleFor(x => x.AccountId).IsValidGuid();
    }
}