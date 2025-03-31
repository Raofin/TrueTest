namespace OPS.Application.Dtos;

public record PageResponse(
    int Index,
    int Size,
    int TotalCount,
    int TotalPages,
    bool HasNext,
    bool HasPrevious
);