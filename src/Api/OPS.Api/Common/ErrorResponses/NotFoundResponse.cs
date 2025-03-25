using Swashbuckle.AspNetCore.Filters;

namespace OPS.Api.Common.ErrorResponses;

public record NotFoundResponse(
    string Type,
    string Title,
    int Status,
    string Detail,
    string TraceId
);

public class NotFoundResponseExample : IExamplesProvider<NotFoundResponse>
{
    public NotFoundResponse GetExamples()
    {
        return new NotFoundResponse(
            Type: "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            Title: "Not Found",
            Status: 404,
            Detail: "A 'Not Found' error has occurred.",
            TraceId: "trace-id"
        );
    }
}