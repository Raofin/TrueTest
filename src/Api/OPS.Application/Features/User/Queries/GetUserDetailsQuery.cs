using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;

namespace OPS.Application.Features.User.Queries;

public record GetUserDetailsQuery : IRequest<ErrorOr<AccountResponse>>;

public class GetUserDetailsQueryHandler(
    IUnitOfWork unitOfWork,
    IUserInfoProvider userInfoProvider) : IRequestHandler<GetUserDetailsQuery, ErrorOr<AccountResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider = userInfoProvider;

    public async Task<ErrorOr<AccountResponse>> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var userAccountId = _userInfoProvider.AccountId();

        var account = await _unitOfWork.Account.GetWithProfile(userAccountId, cancellationToken);

        return account is null
            ? Error.NotFound()
            : account.ToDto();
    }
}