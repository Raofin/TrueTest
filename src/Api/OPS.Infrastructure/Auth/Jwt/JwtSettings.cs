﻿namespace OPS.Infrastructure.Auth.Jwt;

internal class JwtSettings
{
    public string Secret { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int TokenExpirationInMinutes { get; init; }
}