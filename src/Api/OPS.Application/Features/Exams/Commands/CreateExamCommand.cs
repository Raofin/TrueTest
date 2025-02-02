using MediatR;
using ErrorOr;
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
) : IRequest<ErrorOr<ExamResponse>>;

public class CreateExamCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateExamCommand, ErrorOr<ExamResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamResponse>> Handle(CreateExamCommand request, CancellationToken cancellationToken)
    {
        var exam = new Examination
        {
            Title = request.Title,
            Description = request.Description,
            OpensAt = request.OpensAt,
            ClosesAt = request.ClosesAt,
            Duration = request.Duration,
            CreatedAt = DateTime.UtcNow,
            IsActive = request.IsActive,
        };

        _unitOfWork.Exam.Add(exam);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? exam.ToDto()
            : Error.Failure("The exam could not be saved.");
    }
}