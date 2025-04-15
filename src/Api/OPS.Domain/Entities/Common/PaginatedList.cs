using Microsoft.EntityFrameworkCore;

namespace OPS.Domain.Entities.Common
{
    public class PaginatedList<T>(List<T> items, int totalCount, int pageIndex, int pageSize)
    {
        public List<T> Items { get; set; } = items;
        public int TotalCount { get; init; } = totalCount;
        public int PageIndex { get; init; } = pageIndex;
        public int PageSize { get; init; } = pageSize;
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasNextPage => PageIndex < TotalPages;
        public bool HasPreviousPage => PageIndex > 1;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize,
            CancellationToken cancellationToken)
        {
            var totalCount = await source.CountAsync(cancellationToken);
            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, totalCount, pageIndex, pageSize);
        }
    }
}