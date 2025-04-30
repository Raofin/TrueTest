using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace OPS.Api.Transformers;

/// <summary>
/// An <see cref="IOpenApiDocumentTransformer"/> that adds a Bearer security scheme to the OpenAPI document
/// if the "Bearer" authentication scheme is configured. It also adds a security requirement to all operations.
/// </summary>
internal sealed class BearerSecuritySchemeTransformer(
    IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider = authenticationSchemeProvider;

    /// <inheritdoc />
    public async Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var authenticationSchemes = await _authenticationSchemeProvider.GetAllSchemesAsync();

        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            var requirements = new Dictionary<string, OpenApiSecurityScheme>
            {
                ["Bearer"] = new()
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    BearerFormat = "Json Web Token (JWT)",
                    Description =
                        "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJBY2NvdW50SWQiOiIxMTExMTExMS0xMTExLTExMTEtMTExMS0xMTExMTExMTExMTEiLCJVc2VybmFtZSI6ImFkbWluIiwiRW1haWwiOiJhZG1pbkB0cnVldGVzdC50ZWNoIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjpbIkNhbmRpZGF0ZSIsIkFkbWluIl0sIlBlcm1pc3Npb24iOlsiQWNjZXNzT3duRXhhbXMiLCJTdWJtaXRBbnN3ZXJzIiwiUnVuQ29kZSIsIk1hbmFnZU93blByb2ZpbGUiLCJNYW5hZ2VBY2NvdW50cyIsIlZpZXdFeGFtcyIsIk1hbmFnZUV4YW1zIiwiTWFuYWdlUXVlc3Rpb25zIiwiVmlld1N1Ym1pc3Npb25zIiwiUmV2aWV3U3VibWlzc2lvbiJdLCJleHAiOjQ4OTkwMzMxOTUsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3QiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0In0.VBPngQIwsybHrwZPYMA5sNpWew3S9_W7cDdpAD4mDyM"
                }
            };

            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;

            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
                operation.Value.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }
                    ] = Array.Empty<string>()
                });
        }
    }
}