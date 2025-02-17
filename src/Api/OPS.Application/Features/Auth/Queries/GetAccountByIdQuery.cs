using MediatR;
using ErrorOr;
using OPS.Application.Contracts.Auth;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Auth.Queries;

public record GetAccountByIdQuery(Guid AccountId) : IRequest<ErrorOr<AccountResponse>>;

public class GetAccountByIdQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetAccountByIdQuery, ErrorOr<AccountResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<ErrorOr<AccountResponse>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var Account = await _unitOfWork.Account.GetAsync(request.AccountId, cancellationToken);

        return Account is null
            ? Error.NotFound("Account not found.")
            : Account.ToDto();
    }
}