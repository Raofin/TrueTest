using ErrorOr;
using MediatR;
using OPS.Application.Contracts.Auth;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Auth.Queries;

public record GetUpcomingAccounts : IRequest<ErrorOr<List<AccountResponse>>>;

public class GetUpcomingAccountsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUpcomingAccounts, ErrorOr<List<AccountResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<AccountResponse>>> Handle(GetUpcomingAccounts request, CancellationToken cancellationToken)
    {
        var Accounts = await _unitOfWork.Account.GetUpcomingAccountAsync(cancellationToken);

        return Accounts.Select(e => e.ToDto()).ToList();
    }
}