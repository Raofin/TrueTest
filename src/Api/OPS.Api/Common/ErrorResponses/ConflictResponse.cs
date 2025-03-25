using Swashbuckle.AspNetCore.Filters;

namespace OPS.Api.Common.ErrorResponses;

public record ConflictResponse(
    string Type,
    string Title,
    int Status,
    string Detail,
    string TraceId
);

public class ConflictResponseExample : IExamplesProvider<ConflictResponse>
{
    public ConflictResponse GetExamples()
    {
        return new ConflictResponse(
            Type: "https://tools.ietf.org/html/rfc9110#section-15.5.10",
            Title: "Conflict",
            Status: 409,
            Detail: "A 'Conflict' error has occurred.",
            TraceId: "trace-id"
        );
    }
}