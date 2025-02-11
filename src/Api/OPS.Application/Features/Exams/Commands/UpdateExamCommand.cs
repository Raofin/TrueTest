using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.Exams;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Commands;

public record UpdateExamCommand(
    Guid Id,
    string? Title,
    string? Description,
    int? DurationMinutes,
    DateTime? OpensAt,
    DateTime? ClosesAt,
    bool? IsActive,
    bool? IsDeleted
) : IRequest<ErrorOr<ExamResponse>>;

public class UpdateExamCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateExamCommand, ErrorOr<ExamResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamResponse>> Handle(UpdateExamCommand command, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetAsync(command.Id, cancellationToken);

        if (exam is null) return Error.NotFound("Exam was not found");

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
            : Error.Failure("The exam could not be saved.");
    }
}

public class UpdateExamCommandValidator : AbstractValidator<UpdateExamCommand>
{
    public UpdateExamCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.")
            .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Id must be a valid GUID.");

        RuleFor(x => x.Title)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Title))
            .WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(10)
            .When(x => x.DurationMinutes.HasValue)
            .WithMessage("Duration must be more than 10 minutes.");

        RuleFor(x => x.OpensAt)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.OpensAt.HasValue)
            .WithMessage("OpensAt must be in the future.");

        RuleFor(x => x.ClosesAt)
            .GreaterThan(x => x.OpensAt)
            .When(x => x.ClosesAt.HasValue && x.OpensAt.HasValue)
            .WithMessage("ClosesAt must be later than OpensAt.");

        RuleFor(x => x.IsActive)
            .NotNull().When(x => x.IsActive.HasValue)
            .WithMessage("IsActive flag is required when provided.");

        RuleFor(x => x.IsDeleted)
            .NotNull().When(x => x.IsDeleted.HasValue)
            .WithMessage("IsDeleted flag is required when provided.");
    }
}