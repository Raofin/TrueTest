using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Interfaces;

namespace OPS.Application.Features.AiPrompts.Queries;

public record AiExamDescriptionResponse(string Description);

public record AiExamDescriptionQuery(string? Title, string? UserPrompt) : IRequest<ErrorOr<AiExamDescriptionResponse>>;

public class AiExamDescriptionQueryHandler(IAiService aiService)
    : IRequestHandler<AiExamDescriptionQuery, ErrorOr<AiExamDescriptionResponse>>
{
    private readonly IAiService _aiService = aiService;

    public async Task<ErrorOr<AiExamDescriptionResponse>> Handle(AiExamDescriptionQuery request, CancellationToken cancellationToken)
    {
        var prompt = new PromptRequest(
            """
            - Create an exam description following the given title.
            - The description should be related to programming.
            - If a user prompt is provided, use it as a reference.
            - Return JSON: { "description": "string" }
            """,
            [
                $"Title: [{request.UserPrompt}]",
                $"Description: [{request.UserPrompt}]"
            ]
        );

        var response = await _aiService.PromptAsync<AiExamDescriptionResponse>(prompt);

        return response is null
            ? Error.Failure("Failed to get a response from the AI service.")
            : response;
    }
}

public class AiExamDescriptionQueryValidator : AbstractValidator<AiExamDescriptionQuery>
{
    public AiExamDescriptionQueryValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(2000);

        RuleFor(x => x.UserPrompt)
            .MaximumLength(2000);
    }
}