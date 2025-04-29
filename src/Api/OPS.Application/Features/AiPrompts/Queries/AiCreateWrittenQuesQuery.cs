using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Interfaces;

namespace OPS.Application.Features.AiPrompts.Queries;

public record AiWrittenQues(string QuestionStatement);

public record AiCreateWrittenQuesQuery(string? UserPrompt) : IRequest<ErrorOr<AiWrittenQues>>;

public class AiCreateWrittenQuesQueryHandler(IAiService aiService)
    : IRequestHandler<AiCreateWrittenQuesQuery, ErrorOr<AiWrittenQues>>
{
    private readonly IAiService _aiService = aiService;

    public async Task<ErrorOr<AiWrittenQues>> Handle(AiCreateWrittenQuesQuery request,
        CancellationToken cancellationToken)
    {
        var prompt = new PromptRequest(
            """
            - Create a computer science interview question.
            - If a user prompt is provided, use it as a reference.
            - If the user prompt already includes a question, improve it.
            - Use prompts like "Explain" or "Describe" only if the user prompt suggests a topic that needs a long, detailed answer.
            - Return JSON: { "questionStatement": "string" }
            """,
            [
                $"UserPrompt: [{request.UserPrompt}]"
            ]
        );

        var response = await _aiService.PromptAsync<AiWrittenQues>(prompt);

        return response is null
            ? Error.Failure("Failed to get a response from the AI service.")
            : response;
    }
}

// validtor

public class AiCreateWrittenQuesQueryValidator : AbstractValidator<AiCreateWrittenQuesQuery>
{
    public AiCreateWrittenQuesQueryValidator()
    {
        RuleFor(x => x.UserPrompt)
            .MaximumLength(2000);
    }
}