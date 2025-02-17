namespace OPS.Domain.Contracts.Core.EmailSender;

public interface IAccountEmails
{
    void SendOtp(string emailAddress, string code, CancellationToken cancellationToken);
}