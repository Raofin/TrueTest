namespace OPS.Infrastructure.OneCompiler;

internal record CodeRunRequest(
    string Language,
    List<CodeFile> Files,
    string? Stdin
);

internal record CodeFile(
    string Name,
    string Content
);