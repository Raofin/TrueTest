using ErrorOr;
using MediatR;
using OPS.Application.Contracts.Exams;
using OPS.Application.Extensions;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Commands;

public record UpdateExamCommand(
    long ExamId,
    string? Title,
    string? Description,
    DateTime? OpensAt,
    DateTime? ClosesAt,
    int? Duration,
    bool? IsActive,
    bool? IsDeleted
) : IRequest<ErrorOr<ExamResponse>>;

public class UpdateExamCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateExamCommand, ErrorOr<ExamResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamResponse>> Handle(UpdateExamCommand command, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetAsync(command.ExamId, cancellationToken);

        if (exam is null)
        {
            return Error.NotFound("Exam was not found");
        }

        exam.Title = command.Title ?? exam.Title;
        exam.Description = command.Description ?? exam.Description;
        exam.OpensAt = command.OpensAt ?? exam.OpensAt;
        exam.ClosesAt = command.ClosesAt ?? exam.ClosesAt;
        exam.Duration = command.Duration ?? exam.Duration;
        exam.UpdatedAt = DateTime.UtcNow;
        exam.IsActive = command.IsActive ?? exam.IsActive;
        exam.IsDeleted = command.IsDeleted ?? exam.IsDeleted;

        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? exam.ToDto()
            : Error.Failure("The exam could not be saved.");
    }
}