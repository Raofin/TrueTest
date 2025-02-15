using FluentValidation;
using MediatR;
using OPS.Domain;
using OPS.Domain.Contracts.Core.EmailSender;
using OPS.Domain.Entities.Auth;

namespace OPS.Application.Features.User.Commands;

public record SendOtpCommand(string Email) : IRequest<Unit>;

public class SendOtpCommandHandler(
    IUnitOfWork unitOfWork,
    IAccountEmails accountEmails) : IRequestHandler<SendOtpCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAccountEmails _accountEmails = accountEmails;

    public async Task<Unit> Handle(SendOtpCommand request, CancellationToken cancellationToken)
    {
        var code = GenerateOtp();

        var otp = await _unitOfWork.Otp.GetOtpAsync(request.Email, cancellationToken);

        if (otp is not null) _unitOfWork.Otp.Remove(otp);

        _unitOfWork.Otp.Add(new Otp
        {
            Email = request.Email,
            Code = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        });

        await _unitOfWork.CommitAsync(cancellationToken);
        _accountEmails.SendOtp(request.Email, code, cancellationToken);

        return Unit.Value;
    }

    private static string GenerateOtp()
    {
        return new Random().Next(1000, 9999).ToString();
    }
}

public class SendOtpCommandValidator : AbstractValidator<SendOtpCommand>
{
    public SendOtpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$")
            .WithMessage("Invalid email address.");
    }
}