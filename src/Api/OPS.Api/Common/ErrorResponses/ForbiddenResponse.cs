using Swashbuckle.AspNetCore.Filters;

namespace OPS.Api.Common.ErrorResponses;

public record ForbiddenResponse(
    string Type,
    string Title,
    int Status,
    string Detail,
    string TraceId
);

public class ForbiddenResponseExample : IExamplesProvider<ForbiddenResponse>
{
    public ForbiddenResponse GetExamples()
    {
        return new ForbiddenResponse(
            Type: "https://tools.ietf.org/html/rfc9110#section-15.5.4",
            Title: "Forbidden",
            Status: 403,
            Detail: "A 'Forbidden' error has occurred.",
            TraceId: "trace-id"
        );
    }
}