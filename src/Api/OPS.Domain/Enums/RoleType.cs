using System.Text.Json.Serialization;

namespace OPS.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RoleType
{
    Candidate = 1,
    Moderator = 2,
    Admin = 3
}