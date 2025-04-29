namespace OPS.Application.Interfaces.Auth;

public interface IOtpGenerator
{
    string Generate(int length = 4);
}