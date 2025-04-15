using Swashbuckle.AspNetCore.Filters;

namespace OPS.Api.Common.ErrorResponses;

public record ValidationErrorResponse(
    string Type,
    string Title,
    int Status,
    Dictionary<string, List<string>> Errors,
    string TraceId
);

public class ValidationErrorResponseExample : IExamplesProvider<ValidationErrorResponse>
{
    public ValidationErrorResponse GetExamples()
    {
        return new ValidationErrorResponse(
            Type: "ValidationError",
            Title: "One or more validation errors occurred.",
            Status: 400,
            Errors: new Dictionary<string, List<string>>
            {
                { "Field1", ["Error message"] }, { "Field2", ["Error message"] }
            },
            TraceId: "trace-id"
        );
    }
}