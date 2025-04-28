namespace OPS.Application.Interfaces;

public interface IAiService
{
    Task<string?> PromptAsync(string instructionText, List<string> contentParts);
}