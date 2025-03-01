using ErrorOr;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Accounts.Queries;

public record GetAllAccountsByThisMonthQuery : IRequest<ErrorOr<List<AccountResponse>>>;

public class GetAllAccountsByThisMonthQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllAccountsByThisMonthQuery, ErrorOr<List<AccountResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<AccountResponse>>> Handle(GetAllAccountsByThisMonthQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _unitOfWork.Account.GetAsync(cancellationToken);

        return accounts.Select(a => a.ToDto()).ToList();
    }
}