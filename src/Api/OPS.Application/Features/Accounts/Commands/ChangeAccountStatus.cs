using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Accounts.Commands;

public record ChangeAccountStatus(Guid AccountId) : IRequest<ErrorOr<AccountResponse>>;

public class ChangeAccountStatusCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeAccountStatus, ErrorOr<AccountResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<AccountResponse>> Handle(ChangeAccountStatus request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Account.GetAsync(request.AccountId, cancellationToken);

        if (account is null) return Error.NotFound();

        account.IsActive = !account.IsActive;

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? account.ToDto()
            : Error.Failure();
    }
}

public class ChangeAccountStatusValidator : AbstractValidator<ChangeAccountStatus>
{
    public ChangeAccountStatusValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .Must(id => Guid.TryParse(id.ToString(), out _));
    }
}