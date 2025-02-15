using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.Auth.Queries;

public record IsValidOtpQuery(string Email, string Otp) : IRequest<bool>;

public class VerifyOtpQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<IsValidOtpQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<bool> Handle(IsValidOtpQuery request, CancellationToken cancellationToken)
    {
        var isValidOtp = await _unitOfWork.Otp.IsValidOtpAsync(request.Email, request.Otp, cancellationToken);

        return isValidOtp;
    }
}