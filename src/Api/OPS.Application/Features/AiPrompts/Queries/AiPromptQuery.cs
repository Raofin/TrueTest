using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Interfaces;

namespace OPS.Application.Features.AiPrompts.Queries;

public record AiPromptCommand(string Instruction, List<string> Contents) : IRequest<ErrorOr<string>>;

public class AiPromptCommandHandler(IAiService aiService) : IRequestHandler<AiPromptCommand, ErrorOr<string>>
{
    private readonly IAiService _aiService = aiService;

    public async Task<ErrorOr<string>> Handle(AiPromptCommand request, CancellationToken cancellationToken)
    {
        var prompt = new PromptRequest(
            request.Instruction,
            request.Contents
        );

        var response = await _aiService.PromptAsync<string>(prompt);

        return response is null
            ? Error.Failure("Failed to get a response from the AI service.")
            : response;
    }
}

public class AiPromptCommandValidator : AbstractValidator<AiPromptCommand>
{
    public AiPromptCommandValidator()
    {
        RuleFor(x => x.Instruction)
            .MaximumLength(2000);

        RuleForEach(x => x.Contents)
            .MaximumLength(2000);
    }
}