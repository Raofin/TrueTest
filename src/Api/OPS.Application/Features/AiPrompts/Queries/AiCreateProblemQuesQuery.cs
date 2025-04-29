using System.Text.RegularExpressions;
using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Dtos;
using OPS.Application.Interfaces;

namespace OPS.Application.Features.AiPrompts.Queries;

public record AiTestCase(string Input, string Output);

public record AiProblemQuestionResponse(
    string StatementMarkdown,
    List<AiTestCase> TestCases
);

public record AiCreateProblemQuery(string? UserPrompt) : IRequest<ErrorOr<AiProblemQuestionResponse>>;

public class AiCreateProblemQueryHandler(IAiService aiService)
    : IRequestHandler<AiCreateProblemQuery, ErrorOr<AiProblemQuestionResponse>>
{
    private readonly IAiService _aiService = aiService;

    public async Task<ErrorOr<AiProblemQuestionResponse>> Handle(AiCreateProblemQuery request,
        CancellationToken cancellationToken)
    {
        var prompt = new PromptRequest(
            """
            Create a problem-solving question. Test cases input-output must be like Codeforces.

            Include:
            - A short problem statement in Markdown format (like LeetCode).
            - If there is a math equation use KaTex.
            - Max 1 or 2 examples.
            - Constraints.
            - 3-5 test cases.

            * If a user prompt is given, use it as a base and turn it into a proper problem-solving question.
            * If it already has a problem statement, improve it.

            Return JSON:
            {
              "StatementMarkdown": "string",
              "TestCases": [{ "input": "string", "output": "string" }]
            }
            """,
            [
                $"UserPrompt: [{request.UserPrompt}]"
            ]
        );

        var response = await _aiService.PromptAsync<AiProblemQuestionResponse>(prompt);


        return response is null
            ? Error.Failure("Failed to get a response from the AI service.")
            : response;
    }
}

public class AiCreateProblemQueryValidator : AbstractValidator<AiCreateProblemQuery>
{
    public AiCreateProblemQueryValidator()
    {
        RuleFor(x => x.UserPrompt)
            .MaximumLength(2000);
    }
}