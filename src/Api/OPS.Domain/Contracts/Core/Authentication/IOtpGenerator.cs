namespace OPS.Domain.Contracts.Core.Authentication;

public interface IOtpGenerator
{
    string Generate(int length = 4);
}