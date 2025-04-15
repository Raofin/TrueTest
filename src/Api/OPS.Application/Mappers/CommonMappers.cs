using OPS.Application.Dtos;
using OPS.Domain.Entities.Common;

namespace OPS.Application.Mappers;

public static class CommonMappers
{
    public static PageResponse MapToPage<T>(this PaginatedList<T> paginatedList)
    {
        return new PageResponse(
            paginatedList.PageIndex,
            paginatedList.PageSize,
            paginatedList.TotalCount,
            paginatedList.TotalPages,
            paginatedList.HasNextPage,
            paginatedList.HasPreviousPage
        );
    }
}