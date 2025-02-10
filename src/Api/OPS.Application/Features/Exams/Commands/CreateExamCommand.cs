using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Contracts.Exams;
using OPS.Application.Extensions;
using OPS.Domain;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Features.Exams.Commands;

public record CreateExamCommand(
    string Title,
    string Description,
    DateTime OpensAt,
    DateTime ClosesAt,
    int Duration,
    bool IsActive
) : IRequest<ErrorOr<ProfileResponse>>;

public class CreateExamCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateExamCommand, ErrorOr<ProfileResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ProfileResponse>> Handle(CreateExamCommand request, CancellationToken cancellationToken)
    {
        var exam = new Examination
        {
            Title = request.Title,
            DescriptionMarkdown = request.Description,
            OpensAt = request.OpensAt,
            ClosesAt = request.ClosesAt,
            Duration = request.Duration,
            CreatedAt = DateTime.UtcNow,
            IsActive = request.IsActive
        };

        _unitOfWork.Exam.Add(exam);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? exam.ToDto()
            : Error.Failure("The exam could not be saved.");
    }
}

public class CreateExamCommandValidator : AbstractValidator<CreateExamCommand>
{
    public CreateExamCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 100).WithMessage("Title must be between 3 and 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(10, 500).WithMessage("Description must be between 10 and 500 characters.");

        RuleFor(x => x.OpensAt)
            .GreaterThan(DateTime.UtcNow).WithMessage("OpensAt must be in the future.");

        RuleFor(x => x.ClosesAt)
            .GreaterThan(x => x.OpensAt).WithMessage("ClosesAt must be later than OpensAt.");

        RuleFor(x => x.Duration)
            .GreaterThan(5).WithMessage("Duration must be more than 5 minutes.");
    }
}