namespace OPS.Application.Interfaces;

public interface IEmailSender
{
    void SendOtp(string emailAddress, string code, CancellationToken cancellationToken);
    void SendAdminInvitation(List<string> emails, CancellationToken cancellationToken);
    void SendAdminGranted(List<string> emails, CancellationToken cancellationToken);
    void SendExamInvitation(List<string> emails, string examTitle, DateTime startDateTime, int durationMinutes,
        CancellationToken cancellationToken);
}