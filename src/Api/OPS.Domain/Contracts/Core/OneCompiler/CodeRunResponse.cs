namespace OPS.Domain.Contracts.Core.OneCompiler;

public record CodeRunResponse(
    string? Status,
    string? Stdout,
    string? Stdin,
    string? Stderr,
    string? Exception,
    int? ExecutionTime
);