using OPS.Application.Dtos;

namespace OPS.Application.Interfaces;

public interface IAiService
{
    Task<T?> PromptAsync<T>(PromptRequest prompt);
}