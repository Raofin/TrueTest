using System.Security.Cryptography;
using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.CrossCutting.Constants;
using OPS.Domain;
using OPS.Domain.Contracts.Core.EmailSender;
using OPS.Domain.Entities.User;

namespace OPS.Application.Features.Authentication.Commands;

public record SendOtpCommand(string Email) : IRequest<ErrorOr<Unit>>;

public class SendOtpCommandHandler(
    IUnitOfWork unitOfWork,
    IAccountEmails accountEmails) : IRequestHandler<SendOtpCommand, ErrorOr<Unit>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAccountEmails _accountEmails = accountEmails;

    public async Task<ErrorOr<Unit>> Handle(SendOtpCommand request, CancellationToken cancellationToken)
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
        using var rng = RandomNumberGenerator.Create();
        
        var randomBytes = new byte[4];
        rng.GetBytes(randomBytes);

        var otp = BitConverter.ToInt32(randomBytes, 0) & 0x7FFFFFFF;
        otp = otp % 9000 + 1000;
        
        return otp.ToString();
    }
}

public class SendOtpCommandValidator : AbstractValidator<SendOtpCommand>
{
    public SendOtpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .Matches(ValidationConstants.EmailRegex);
    }
}