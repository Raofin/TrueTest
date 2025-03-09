using OPS.Domain.Enums;

namespace OPS.Application.Contracts.Dtos;

public record ProblemSubmitResponse(
    Guid Id,
    string Code,
    int Attempts,
    decimal? Score,
    ProgLanguageType ProgLanguageType,
    List<TestCaseOutputResponse> TestCaseOutputs
);

public record TestCaseOutputResponse(
    Guid TestCaseId,
    bool IsAccepted,
    string Input,
    string ExpectedOutput,
    string ReceivedOutput
);