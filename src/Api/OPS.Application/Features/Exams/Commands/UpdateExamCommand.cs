using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Application.Contracts.Dtos;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Commands;

public record UpdateExamCommand(
    Guid ExamId,
    string? Title,
    string? Description,
    int? DurationMinutes,
    DateTime? OpensAt,
    DateTime? ClosesAt,
    bool? IsActive,
    bool? IsDeleted) : IRequest<ErrorOr<ExamResponse>>;

public class UpdateExamCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateExamCommand, ErrorOr<ExamResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamResponse>> Handle(UpdateExamCommand command, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetAsync(command.ExamId, cancellationToken);

        if (exam is null)
            return Error.NotFound();

        exam.Title = command.Title ?? exam.Title;
        exam.DescriptionMarkdown = command.Description ?? exam.DescriptionMarkdown;
        exam.DurationMinutes = command.DurationMinutes ?? exam.DurationMinutes;
        exam.OpensAt = command.OpensAt ?? exam.OpensAt;
        exam.ClosesAt = command.ClosesAt ?? exam.ClosesAt;
        exam.UpdatedAt = DateTime.UtcNow;
        exam.IsActive = command.IsActive ?? exam.IsActive;
        exam.IsDeleted = command.IsDeleted ?? exam.IsDeleted;

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? exam.ToDto()
            : Error.Failure();
    }
}

public class UpdateExamCommandValidator : AbstractValidator<UpdateExamCommand>
{
    public UpdateExamCommandValidator()
    {
        RuleFor(x => x.ExamId)
            .NotEmpty()
            .Must(id => Guid.TryParse(id.ToString(), out _));

        RuleFor(x => x.Title)
            .MaximumLength(10)
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Description)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(10)
            .When(x => x.DurationMinutes.HasValue);

        RuleFor(x => x.OpensAt)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.OpensAt.HasValue);

        RuleFor(x => x.ClosesAt)
            .GreaterThan(x => x.OpensAt)
            .When(x => x.ClosesAt.HasValue && x.OpensAt.HasValue);

        RuleFor(x => x.IsActive)
            .NotNull().When(x => x.IsActive.HasValue);

        RuleFor(x => x.IsDeleted)
            .NotNull().When(x => x.IsDeleted.HasValue);
    }
}