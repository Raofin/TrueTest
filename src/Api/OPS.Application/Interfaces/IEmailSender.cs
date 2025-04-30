namespace OPS.Application.Interfaces;

/// <summary>
/// Defines the contract for sending various types of emails.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends a One-Time Password (OTP) to the specified email address.
    /// </summary>
    /// <param name="emailAddress">The recipient's email address.</param>
    /// <param name="code">The OTP code to be sent.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    void SendOtp(string emailAddress, string code, CancellationToken cancellationToken);

    /// <summary>
    /// Sends an invitation email to a list of email addresses for admin access.
    /// </summary>
    /// <param name="emails">A list of recipient email addresses.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    void SendAdminInvitation(List<string> emails, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a notification email to a list of email addresses informing them that admin access has been granted.
    /// </summary>
    /// <param name="emails">A list of recipient email addresses.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    void SendAdminGranted(List<string> emails, CancellationToken cancellationToken);

    /// <summary>
    /// Sends an invitation email to a list of email addresses for an exam.
    /// </summary>
    /// <param name="emails">A list of recipient email addresses.</param>
    /// <param name="examTitle">The title of the exam.</param>
    /// <param name="startDateTime">The date and time when the exam starts.</param>
    /// <param name="durationMinutes">The duration of the exam in minutes.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    void SendExamInvitation(List<string> emails, string examTitle, DateTime startDateTime, int durationMinutes,
        CancellationToken cancellationToken);
}