using System.Text.Json.Serialization;

namespace OPS.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProgLanguageType
{
    Python = 1,
    C = 2,
    Cpp = 3,
    Java = 4,
    JavaScript = 5,
    TypeScript = 6,
    Csharp = 7,
    Ruby = 8,
    Go = 9,
    PHP = 10
}