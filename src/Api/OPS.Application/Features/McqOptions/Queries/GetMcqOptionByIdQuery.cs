using ErrorOr;
using MediatR;
using FluentValidation;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.McqOptions.Queries;

public record GetMcqOptionByIdQuery(Guid McqOptionId) : IRequest<ErrorOr<McqOptionResponse>>;

public class GetMcqOptionByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetMcqOptionByIdQuery, ErrorOr<McqOptionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<McqOptionResponse>> Handle(GetMcqOptionByIdQuery request, CancellationToken cancellationToken)
    {
        var mcqOption = await _unitOfWork.McqOption.GetAsync(request.McqOptionId, cancellationToken);

        return mcqOption is null
            ? Error.NotFound()
            : mcqOption.ToDto();
    }
}

public class GetMcqOptionnByIdQueryValidator : AbstractValidator<GetMcqOptionByIdQuery>
{
    public GetMcqOptionnByIdQueryValidator()
    {
        RuleFor(x => x.McqOptionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}