using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OPS.Application.Interfaces;
using OPS.Infrastructure.Gemini.Configuration;
using OPS.Infrastructure.Gemini.Refit;
using Serilog;

namespace OPS.Infrastructure.Gemini;

internal class GeminiService(IGeminiClient geminiClient, IOptions<GeminiSettings> settings) : IAiService
{
    private readonly IGeminiClient _geminiClient = geminiClient;
    private readonly string _apiKey = settings.Value.ApiKey;
    private readonly string _model = settings.Value.Model;

    public async Task<string?> PromptAsync(string instructionText, List<string> contentParts)
    {
        try
        {
            var geminiRequest = new
            {
                systemInstruction = new
                {
                    parts = new[] { new { text = instructionText } }
                },
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = contentParts.Select(part => new { text = part }).ToArray()
                    }
                }
            };

            var jsonPayload = JsonConvert.SerializeObject(geminiRequest);

            var response = await _geminiClient.SendRequestAsync(jsonPayload, _model, _apiKey);

            if (string.IsNullOrEmpty(response))
            {
                Log.Error("Received empty response from Gemini API.");
                return null;
            }

            var parsedResponse = JObject.Parse(response);
            var error = parsedResponse["error"];

            if (error != null)
            {
                var errorMessage = error["message"]?.ToString();
                Log.Error("Gemini API returned an error: {ErrorMessage}", errorMessage);
                return null;
            }

            var reviewText = parsedResponse["candidates"]?
                .FirstOrDefault()?
                .SelectToken("content.parts[0].text")?.ToString();

            if (!string.IsNullOrEmpty(reviewText))
            {
                return new Regex(@"```json\n|\n```").Replace(reviewText, string.Empty);
            }

            Log.Error("Review text not found in the response.");
            return null;

        }
        catch (Exception ex)
        {
            Log.Error("An error occurred while processing the Gemini request: {Message}", ex.Message);
            return null;
        }
    }
}