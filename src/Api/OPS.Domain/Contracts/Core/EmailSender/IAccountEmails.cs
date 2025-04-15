namespace OPS.Domain.Contracts.Core.EmailSender;

public interface IAccountEmails
{
    void SendOtp(string emailAddress, string code, CancellationToken cancellationToken);
    void SendAdminInvitation(List<string> emails, CancellationToken cancellationToken);
    void SendAdminGranted(List<string> emails, CancellationToken cancellationToken);
    void SendExamInvitation(List<string> emails, string examTitle, DateTime startDateTime, int durationMinutes,
        CancellationToken cancellationToken);
}