﻿using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;
using OPS.Domain.Constants;

namespace OPS.Application.Features.Authentication.Queries;

public record IsValidOtpQuery(string Email, string Otp) : IRequest<ErrorOr<bool>>;

public class VerifyOtpQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<IsValidOtpQuery, ErrorOr<bool>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<bool>> Handle(IsValidOtpQuery request, CancellationToken cancellationToken)
    {
        var isValidOtp =
            await _unitOfWork.Otp.IsValidOtpAsync(
                request.Email,
                request.Otp,
                cancellationToken
            );

        return isValidOtp;
    }
}

public class IsValidOtpQueryValidator : AbstractValidator<IsValidOtpQuery>
{
    public IsValidOtpQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .Matches(ValidationConstants.EmailRegex);

        RuleFor(x => x.Otp)
            .NotEmpty()
            .Length(4);
    }
}