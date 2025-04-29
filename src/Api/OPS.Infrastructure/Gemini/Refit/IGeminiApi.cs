using Refit;

namespace OPS.Infrastructure.Gemini.Refit;

public interface IGeminiClient
{
    [Post("/{model}:generateContent")]
    Task<string> SendRequestAsync(
        [Body] string jsonPayload,
        [AliasAs("model")] string model,
        [Query] string key
    );
}