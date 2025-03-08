using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.McqOptions.Commands;

public record UpdateMcqOptionCommand(
      Guid Id,
      string Option1,
      string Option2,
      string Option3,
      string Option4,
      bool isMultiSelect,
      string AnswerOptions
    ) : IRequest<ErrorOr<McqOptionResponse>>;

public class UpdateMcqOptionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateMcqOptionCommand, ErrorOr<McqOptionResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<McqOptionResponse>> Handle(UpdateMcqOptionCommand command,
        CancellationToken cancellationToken)
    {
        var mcqOption = await _unitOfWork.McqOption.GetAsync(command.Id, cancellationToken);
        
        if (mcqOption is null) return Error.NotFound();

        mcqOption.Option1 = command.Option1 ?? mcqOption.Option1;
        mcqOption.Option2 = command.Option2 ?? mcqOption.Option2;
        mcqOption.Option3 = command.Option3 ?? mcqOption.Option3;
        mcqOption.Option4 = command.Option4 ?? mcqOption.Option4;
        mcqOption.IsMultiSelect = mcqOption.IsMultiSelect;
        mcqOption.AnswerOptions = command.AnswerOptions ?? mcqOption.AnswerOptions;
        mcqOption.UpdatedAt = DateTime.UtcNow;

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? mcqOption.ToDto()
            : Error.Failure();
    }
}

public class UpdateMcqOptionCommandValidator : AbstractValidator<UpdateMcqOptionCommand>
{
    public UpdateMcqOptionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(id => id != Guid.Empty);

        RuleFor(x => x.AnswerOptions)
            .NotEmpty()
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.AnswerOptions));
    }
}