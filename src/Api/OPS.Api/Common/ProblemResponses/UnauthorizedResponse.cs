using Swashbuckle.AspNetCore.Filters;

namespace OPS.Api.Common.ProblemResponses;

public record UnauthorizedResponse(
    string Type,
    string Title,
    int Status,
    string Detail,
    string TraceId
);

public class UnauthorizedResponseExample : IExamplesProvider<UnauthorizedResponse>
{
    public UnauthorizedResponse GetExamples()
    {
        return new UnauthorizedResponse(
            Type: "https://tools.ietf.org/html/rfc9110#section-15.5.4",
            Title: "Forbidden",
            Status: 403,
            Detail: "An 'Unauthorized' error has occurred.",
            TraceId: "trace-id"
        );
    }
}