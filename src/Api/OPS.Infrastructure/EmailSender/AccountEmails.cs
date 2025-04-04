using FluentEmail.Core;
using FluentEmail.Core.Models;
using OPS.Application.CrossCutting.Constants;
using OPS.Domain.Contracts.Core.EmailSender;
using Serilog;

namespace OPS.Infrastructure.EmailSender;

public class AccountEmails(IFluentEmail fluentEmail) : IAccountEmails
{
    private readonly IFluentEmail _fluentEmail = fluentEmail;

    public void SendOtp(string emailAddress, string code, CancellationToken cancellationToken)
    {
        var email = _fluentEmail
            .To(emailAddress)
            .Subject($"{ProjectConstants.ProjectName} - Email Verification")
            .Body($"""
                     <body style='font-family: Inter, Arial, sans-serif;'>
                       <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                         <h2 style='color: #333; text-align: center;'>Email Verification</h2>
                         <p>Thank you for signing up with {ProjectConstants.ProjectName}! Please use the following code to verify your account.</p>
                         <div style='text-align: center; line-height: 25px;'>
                           <div style='margin-bottom: 5px;'>
                               <span>Verification Code</span><br>
                               <span style='font-size: 20px; font-weight: 700;'>{code}</span><br>
                               <span>(This code is valid for 5 minutes)</span>
                           </div>
                         </div>
                         <p>If you did not request this OTP, please ignore this email.</p>
                         <p>Best,<br>{ProjectConstants.ProjectName} Team</p>
                       </div>
                     </body>
                   """,
                isHtml: true);

        Send(email, cancellationToken);
    }

    public void SendAdminInvitation(List<string> emailAddresses, CancellationToken cancellationToken)
    {
        emailAddresses.ForEach(e => { AdminInvitation(e, cancellationToken); });
    }

    private void AdminInvitation(string emailAddress, CancellationToken cancellationToken)
    {
        var email = _fluentEmail
            .To(emailAddress)
            .Subject($"{ProjectConstants.ProjectName} - Admin Invitation")
            .Body($"""
                     <body style='font-family: Inter, Arial, sans-serif;'>
                       <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                         <h2 style='color: #333; text-align: center;'>You're Invited to Join {ProjectConstants.ProjectName}</h2>
                         <p>Hello,</p>
                         <p>You have been invited to join {ProjectConstants.ProjectName} as an administrator.</p>
                         <p>Please register your account to gain admin access.</p>
                         <p>Once registered, you will be able to manage the platform as an administrator.</p>
                         <p>If you did not expect this invitation, please ignore this email.</p>
                         <p>Best,<br>{ProjectConstants.ProjectName} Team</p>
                       </div>
                     </body>
                   """,
                isHtml: true);

        Send(email, cancellationToken);
    }

    public void SendAdminGranted(List<string> emailAddresses, CancellationToken cancellationToken)
    {
        emailAddresses.ForEach(e => { AdminAccessGranted(e, cancellationToken); });
    }

    private void AdminAccessGranted(string emailAddress, CancellationToken cancellationToken)
    {
        var email = _fluentEmail
            .To(emailAddress)
            .Subject($"{ProjectConstants.ProjectName} - Admin Access Granted")
            .Body($"""
                     <body style='font-family: Inter, Arial, sans-serif;'>
                       <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                         <h2 style='color: #333; text-align: center;'>Admin Access Granted</h2>
                         <p>Hello,</p>
                         <p>You have been granted admin access to {ProjectConstants.ProjectName}.</p>
                         <p>Please log in to your account and start managing the platform!</p>
                         <p>If you were not expecting this access, please contact support.</p>
                         <p>Best,<br>{ProjectConstants.ProjectName} Team</p>
                       </div>
                     </body>
                   """,
                isHtml: true);

        Send(email, cancellationToken);
    }

    public void SendExamInvitation(List<string> emails, string examTitle, DateTime startDateTime, int durationMinutes,
        CancellationToken cancellationToken)
    {
        var hours = durationMinutes / 60;
        var minutes = durationMinutes % 60;

        var duration = hours > 0
            ? $"{hours}h {minutes}m"
            : $"{minutes} minutes";

        var email = _fluentEmail
            .BCC(emails.Select(e => new Address(e)))
            .Subject($"{examTitle} - Exam Invitation")
            .Body($"""
                   <body style='font-family: Inter, Arial, sans-serif;'>
                     <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                       <h2 style='color: #333; text-align: center;'>Exam Invitation: {examTitle}</h2>
                       <p>Dear Participant,</p>
                       <p>You are invited to participate in the <strong>{examTitle}</strong> exam on {ProjectConstants.ProjectName}.</p>
                       <p>The exam is scheduled to begin on <strong>{startDateTime:dddd, MMMM dd, yyyy hh:mm tt}</strong>, and the 
                       duration is <strong>{duration}.</strong>.</p>
                       <p>Please <a href='#'>log in</a> to your account to access the exam when it becomes available.</p>
                       <p>If you do not yet have an account, you can create one <a href='#'>here</a>.</p>
                       <p>We encourage you to prepare accordingly.</p>
                       <p>Best regards,<br>{ProjectConstants.ProjectName} Team</p>
                     </div>
                   </body>
                   """,
                isHtml: true);

        Send(email, cancellationToken);
    }

    private static void Send(IFluentEmail email, CancellationToken cancellationToken)
    {
        try
        {
            email.SendAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error while sending email: {Message}", e.Message);
        }
    }
}