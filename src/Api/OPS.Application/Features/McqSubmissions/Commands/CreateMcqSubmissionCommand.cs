using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos    ;
using OPS.Domain;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Features.McqSubmissions.Commands;

public record CreateMcqSubmissionCommand(
    string AnswerOptions,
    Guid AccountId,
    Guid McqOptionId
    ) : IRequest<ErrorOr<McqSubmissionResponse>>;

public class CreateMcqSubmissionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateMcqSubmissionCommand, ErrorOr<McqSubmissionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<McqSubmissionResponse>> Handle(CreateMcqSubmissionCommand request,
        CancellationToken cancellationToken)
    {
        var accountExists = await _unitOfWork.Account.GetAsync(request.AccountId, cancellationToken);
        if (accountExists == null) return Error.NotFound();

        var mcqOptionExists = await _unitOfWork.McqOption.GetAsync(request.McqOptionId, cancellationToken);     
        if(mcqOptionExists == null) return Error.NotFound();        

        var mcqSubmission = new McqSubmission
        {
            AccountId = request.AccountId,
            AnswerOptions = request.AnswerOptions,
            McqOptionId = request.McqOptionId
        };

        _unitOfWork.McqSubmission.Add(mcqSubmission);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? mcqSubmission.ToDto()
            : Error.Failure();
    }
}

public class CreateMcqSubmissionCommandValidator : AbstractValidator<CreateMcqSubmissionCommand>
{
    public CreateMcqSubmissionCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);

        RuleFor(x => x.McqOptionId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);

        RuleFor(x => x.AnswerOptions).NotEmpty();
    }
}