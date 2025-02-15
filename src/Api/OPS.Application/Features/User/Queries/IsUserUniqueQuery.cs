using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.User.Queries;

public record IsUserUniqueQuery(string? Username, string? Email) : IRequest<bool>;

public class IsUserUniqueQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<IsUserUniqueQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<bool> Handle(IsUserUniqueQuery request, CancellationToken cancellationToken)
    {
        var isUserUnique = await _unitOfWork.Account.IsUsernameOrEmailUniqueAsync(
            request.Username, request.Email, cancellationToken);

        return isUserUnique;
    }
}