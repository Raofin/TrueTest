using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.Auth.Queries;

public record IsUserUniqueQuery(string? Username, string? Email) : IRequest<ErrorOr<bool>>;

public class IsUserUniqueQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<IsUserUniqueQuery, ErrorOr<bool>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<bool>> Handle(IsUserUniqueQuery request, CancellationToken cancellationToken)
    {
        var isUserUnique =
            await _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(
                request.Username,
                request.Email,
                cancellationToken
            );

        return isUserUnique;
    }
}

public class IsUserUniqueQueryValidator : AbstractValidator<IsUserUniqueQuery>
{
    public IsUserUniqueQueryValidator()
    {
        RuleFor(x => x.Username)
            .MinimumLength(4)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.Username));

        RuleFor(x => x.Email)
            .Matches(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.Username) || !string.IsNullOrEmpty(x.Email))
            .WithMessage("Either Username or Email must be provided.")
            .OverridePropertyName("UsernameOrEmail");
    }
}