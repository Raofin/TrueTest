using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Interfaces;
using OPS.Domain.Enums;

namespace OPS.Application.Features.Candidates.Commands;

public record CodeRunCommand(string Code, string? Input, LanguageId LanguageId) : IRequest<CodeRunResponse>;

public class CodeRunCommandHandler(IOneCompilerService oneCompiler)
    : IRequestHandler<CodeRunCommand, CodeRunResponse>
{
    private readonly IOneCompilerService _oneCompiler = oneCompiler;

    public async Task<CodeRunResponse> Handle(CodeRunCommand request, CancellationToken cancellationToken)
    {
        return await _oneCompiler.CodeRunAsync(request.LanguageId, request.Code, request.Input);
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