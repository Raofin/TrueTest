using ErrorOr;
using MediatR;
using OPS.Application.Contracts.Auth;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Auth.Queries;

public record GetAllAccountsQuery : IRequest<ErrorOr<List<AccountResponse>>>;

public class GetAllAccountsQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetAllAccountsQuery, ErrorOr<List<AccountResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<AccountResponse>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var Accounts = await _unitOfWork.Account.GetAsync(cancellationToken);

        return Accounts.Select(e => e.ToDto()).ToList();
    }
}