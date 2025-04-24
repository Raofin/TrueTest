using System.Diagnostics.CodeAnalysis;
using ErrorOr;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;

namespace OPS.Application.Features.User.Queries;

[ExcludeFromCodeCoverage]
public record GetUserDetailsQuery : IRequest<ErrorOr<AccountWithDetailsResponse>>;

public class GetUserDetailsQueryHandler(
    IUnitOfWork unitOfWork,
    IUserInfoProvider userInfoProvider) : IRequestHandler<GetUserDetailsQuery, ErrorOr<AccountWithDetailsResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<AccountWithDetailsResponse>> Handle(GetUserDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var userAccountId = _userInfoProvider.AccountId();

        var account = await _unitOfWork.Account.GetWithDetails(userAccountId, cancellationToken);

        return account is null
            ? Error.NotFound()
            : account.MapToDtoWithDetails();
    }
}