using System.Diagnostics.CodeAnalysis;
using ErrorOr;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Interfaces.Auth;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.User.Queries;

[ExcludeFromCodeCoverage]
public record GetUserDetailsQuery : IRequest<ErrorOr<AccountWithDetailsResponse>>;

public class GetUserDetailsQueryHandler(IUnitOfWork unitOfWork, IUserProvider userProvider)
    : IRequestHandler<GetUserDetailsQuery, ErrorOr<AccountWithDetailsResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserProvider _userProvider = userProvider;

    public async Task<ErrorOr<AccountWithDetailsResponse>> Handle(GetUserDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var userAccountId = _userProvider.AccountId();

        var account = await _unitOfWork.Account.GetWithDetails(userAccountId, cancellationToken);

        return account is null
            ? Error.NotFound()
            : account.MapToDtoWithDetails();
    }
}