namespace OPS.Application.Dtos;

public record PromptRequest(
    string Instruction,
    List<string> Contents
);