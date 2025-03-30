using ErrorOr;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.Accounts.Queries;

public record GetAllAccountsQuery : IRequest<ErrorOr<List<AccountWithDetailsResponse>>>;

public class GetAllAccountsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllAccountsQuery, ErrorOr<List<AccountWithDetailsResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<AccountWithDetailsResponse>>> Handle(GetAllAccountsQuery request,
        CancellationToken cancellationToken)
    {
        var accounts = await _unitOfWork.Account.GetAllWithDetails(cancellationToken);

        return accounts.Select(a => a.MapToDtoWithDetails()).ToList();
    }
}