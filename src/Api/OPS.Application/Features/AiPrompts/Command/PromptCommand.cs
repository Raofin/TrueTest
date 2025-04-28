using ErrorOr;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Interfaces;

namespace OPS.Application.Features.AiPrompts.Command;

public record PromptCommand(PromptRequest Prompt) : IRequest<ErrorOr<string>>;

public class PromptCommandHandler(IAiService aiService) : IRequestHandler<PromptCommand, ErrorOr<string>>
{
    private readonly IAiService _aiService = aiService;

    public async Task<ErrorOr<string>> Handle(PromptCommand request, CancellationToken cancellationToken)
    {
        var response = await _aiService.PromptAsync(request.Prompt.Instruction, request.Prompt.Contents);

        return response is null
            ? Error.Failure("Failed to get a response from the AI service.")
            : response;
    }
}