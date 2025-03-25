using Swashbuckle.AspNetCore.Filters;

namespace OPS.Api.Common.ProblemResponses;

public record ExceptionResponse(
    string Type,
    string Title,
    int Status,
    string Detail,
    string TraceId,
    ExceptionDetails Exception
);

public record ExceptionDetails(
    string Details,
    Dictionary<string, List<string>> Headers,
    string Path,
    string Endpoint,
    Dictionary<string, string> RouteValues
);

public class ExceptionResponseExample : IExamplesProvider<ExceptionResponse>
{
    public ExceptionResponse GetExamples()
    {
        return new ExceptionResponse(
            Type: "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            Title: "System.Exception",
            Status: 500,
            Detail: "Exception of type 'System.Exception' was thrown.",
            TraceId: "trace-id",
            Exception: new ExceptionDetails(
                Details: "Exception details",
                Headers: new Dictionary<string, List<string>>
                {
                    { "HeaderProp1", ["Value"] },
                    { "HeaderProp2", ["Value"] }
                },
                Path: "/api/...",
                Endpoint: "Endpoint method name",
                RouteValues: new Dictionary<string, string>
                {
                    { "action", "action" },
                    { "controller", "controller" }
                }
            )
        );
    }
}