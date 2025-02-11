using ErrorOr;
using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.Auth.Commands;

public record DeleteAccountCommand(Guid AccountId) : IRequest<ErrorOr<Success>>;

public class DeleteAccountCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteAccountCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var Account = await _unitOfWork.Account.GetAsync(request.AccountId, cancellationToken);

        if (Account is null) return Error.NotFound("Account was not found");

        _unitOfWork.Account.Remove(Account);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? Result.Success
            : Error.Failure("The Account could not be deleted.");
    }
}