using ErrorOr;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Accounts.Queries;

public record PaginatedAccountResponse(PageResponse Page, List<AccountWithDetailsResponse> Accounts);

public record GetAllAccountsQuery(int PageIndex, int PageSize, string? SearchTerm, RoleType? Role)
    : IRequest<ErrorOr<PaginatedAccountResponse>>;

public class GetAllAccountsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllAccountsQuery, ErrorOr<PaginatedAccountResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<PaginatedAccountResponse>> Handle(GetAllAccountsQuery request,
        CancellationToken cancellationToken)
    {
        var paginatedAccounts = await _unitOfWork.Account.GetAllWithDetails(
            request.PageIndex, request.PageSize, request.SearchTerm, request.Role, cancellationToken);

        return new PaginatedAccountResponse(
            paginatedAccounts.MapToPage(),
            paginatedAccounts.Items.Select(a => a.MapToDtoWithDetails()).ToList()
        );
    }
}