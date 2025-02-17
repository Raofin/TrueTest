using System.Text.Json.Serialization;

namespace OPS.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuestionType
{
    ProblemSolving = 1,
    Written = 2,
    MCQ = 3
}