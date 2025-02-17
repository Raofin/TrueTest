using System.Text.Json.Serialization;

namespace OPS.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DifficultyType
{
    Easy = 1,
    Medium = 2,
    Hard = 3
}