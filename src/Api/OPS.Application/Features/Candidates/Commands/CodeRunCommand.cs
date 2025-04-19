using FluentValidation;
using MediatR;
using OPS.Domain.Contracts.Core.OneCompiler;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Candidates.Commands;

public record CodeRunCommand(string Code, string? Input, LanguageId LanguageId) : IRequest<CodeRunResponse>;

public class CodeRunCommandHandler(IOneCompilerApiService oneCompilerApi)
    : IRequestHandler<CodeRunCommand, CodeRunResponse>
{
    private readonly IOneCompilerApiService _oneCompilerApi = oneCompilerApi;

    public async Task<CodeRunResponse> Handle(CodeRunCommand request, CancellationToken cancellationToken)
    {
        return await _oneCompilerApi.CodeRunAsync(request.LanguageId, request.Code, request.Input);
    }
}

public class CodeRunCommandValidator : AbstractValidator<CodeRunCommand>
{
    public CodeRunCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.LanguageId).IsInEnum();
    }
}