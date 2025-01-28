using ErrorOr;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Interfaces.Repositories;
using OPS.Service.Contract;
using OPS.Service.Dtos;

namespace OPS.Service.Implementation;

internal class ExamService(IUnitOfWork unitOfWork, IExamRepository examRepository) : IExamService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IExamRepository _examRepository = examRepository;

    public async Task<ErrorOr<ExamDto>> GetExamAsync(long examId)
    {
        var exam = await _examRepository.GetAsync(examId);

        return exam is null
            ? Error.NotFound()
            : ToDto(exam);
    }

    public async Task<ErrorOr<ExamDto>> CreateExamAsync(ExamDto dto)
    {
        var exam = new Examination
        {
            Title = dto.Title,
            Description = dto.Description,
            OpensAt = dto.OpensAt,
            ClosesAt = dto.ClosesAt,
            Duration = dto.Duration,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = dto.IsActive,
            IsDeleted = dto.IsDeleted
        };

        _examRepository.Add(exam);
        var result = await _unitOfWork.CommitAsync();

        return result > 0
            ? ToDto(exam)
            : Error.Failure("The exam could not be saved.");
    }

    private static ExamDto ToDto(Examination exam)
    {
        return new ExamDto
        {
            ExamId = exam.ExamId,
            Title = exam.Title,
            Description = exam.Description,
            OpensAt = exam.OpensAt,
            ClosesAt = exam.ClosesAt,
            Duration = exam.Duration,
            CreatedAt = exam.CreatedAt,
            UpdatedAt = exam.UpdatedAt,
            IsActive = exam.IsActive,
            IsDeleted = exam.IsDeleted
        };
    }
}
