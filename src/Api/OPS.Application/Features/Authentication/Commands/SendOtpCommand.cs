using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common.Constants;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Contracts.Core.EmailSender;
using OPS.Domain.Entities.User;

namespace OPS.Application.Features.Authentication.Commands;

public record SendOtpCommand(string Email) : IRequest<ErrorOr<Unit>>;

public class SendOtpCommandHandler(
    IAccountEmails accountEmails,
    IOtpGenerator otpGenerator,
    IUnitOfWork unitOfWork) : IRequestHandler<SendOtpCommand, ErrorOr<Unit>>
{
    private readonly IAccountEmails _accountEmails = accountEmails;
    private readonly IOtpGenerator _otpGenerator = otpGenerator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Unit>> Handle(SendOtpCommand request, CancellationToken cancellationToken)
    {
        var code = _otpGenerator.Generate();

        var otp = await _unitOfWork.Otp.GetOtpAsync(request.Email, cancellationToken);
        if (otp is not null) _unitOfWork.Otp.Remove(otp);

        _unitOfWork.Otp.Add(
            new Otp
            {
                Email = request.Email,
                Code = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            }
        );

        await _unitOfWork.CommitAsync(cancellationToken);
        _accountEmails.SendOtp(request.Email, code, cancellationToken);

        return Unit.Value;
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