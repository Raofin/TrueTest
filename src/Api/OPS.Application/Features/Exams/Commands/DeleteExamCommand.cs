using ErrorOr;
using MediatR;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Commands;

public record DeleteExamCommand(Guid Id) : IRequest<ErrorOr<Success>>;

public class DeleteExamCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteExamCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetAsync(request.Id, cancellationToken);

        if (exam is null) return Error.NotFound("Exam was not found");

        _unitOfWork.Exam.Remove(exam);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return result > 0
            ? Result.Success
            : Error.Failure("The exam could not be deleted.");
    }
}