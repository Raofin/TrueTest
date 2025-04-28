namespace OPS.Application.Dtos;

public record CodeRunResponse(
    string? Status,
    string? Stdout,
    string? Stdin,
    string? Stderr,
    string? Exception,
    int? ExecutionTime
);