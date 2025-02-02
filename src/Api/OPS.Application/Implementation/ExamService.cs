using ErrorOr;
using OPS.Application.Contracts;
using OPS.Application.Dtos;
using OPS.Domain;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Implementation;

internal class ExamService(IUnitOfWork unitOfWork) : IExamService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<List<ExamDto>>> GetAsync()
    {
        var exams = await _unitOfWork.Exam.GetAsync();

        return exams.Select(ToDto).ToList();
    }

    public async Task<ErrorOr<ExamDto>> GetByIdAsync(long examId)
    {
        var exam = await _unitOfWork.Exam.GetAsync(examId);

        return exam is null
            ? Error.NotFound()
            : ToDto(exam);
    }

    public async Task<ErrorOr<List<ExamDto>>> GetUpcomingExamsAsync()
    {
        var exams = await _unitOfWork.Exam.GetUpcomingExamsAsync();

        return exams.Select(ToDto).ToList();
    }

    public async Task<ErrorOr<ExamDto>> CreateAsync(ExamCreateDto dto)
    {
        var exam = new Examination
        {
            Title = dto.Title,
            Description = dto.Description,
            OpensAt = dto.OpensAt,
            ClosesAt = dto.ClosesAt,
            Duration = dto.Duration,
            CreatedAt = DateTime.UtcNow,
            IsActive = dto.IsActive,
        };

        _unitOfWork.Exam.Add(exam);
        var result = await _unitOfWork.CommitAsync();

        return result > 0
            ? ToDto(exam)
            : Error.Failure("The exam could not be saved.");
    }

    public async Task<ErrorOr<ExamDto>> UpdateAsync(ExamUpdateDto dto)
    {
        var exam = await _unitOfWork.Exam.GetAsync(dto.ExamId);

        if (exam == null)
        {
            return Error.NotFound("Exam was not found");
        }

        exam.Title = dto.Title ?? exam.Title;
        exam.Description = dto.Description ?? exam.Description;
        exam.OpensAt = dto.OpensAt ?? exam.OpensAt;
        exam.ClosesAt = dto.ClosesAt ?? exam.ClosesAt;
        exam.Duration = dto.Duration ?? exam.Duration;
        exam.UpdatedAt = DateTime.UtcNow;
        exam.IsActive = dto.IsActive ?? exam.IsActive;
        exam.IsDeleted = dto.IsDeleted ?? exam.IsDeleted;

        var result = await _unitOfWork.CommitAsync();

        return result > 0
            ? ToDto(exam)
            : Error.Failure("The exam could not be saved.");
    }

    public async Task<ErrorOr<Success>> DeleteAsync(long examId)
    {
        var exam = await _unitOfWork.Exam.GetAsync(examId);

        if (exam == null)
        {
            return Error.NotFound("Exam was not found");
        }

        _unitOfWork.Exam.Remove(exam);
        var result = await _unitOfWork.CommitAsync();

        return result > 0
            ? Result.Success
            : Error.Failure("The exam could not be deleted.");
    }

    private static ExamDto ToDto(Examination exam)
    {
        return new ExamDto(
            exam.ExamId,
            exam.Title,
            exam.Description,
            exam.OpensAt,
            exam.ClosesAt,
            exam.Duration,
            exam.CreatedAt,
            exam.UpdatedAt,
            exam.IsActive,
            exam.IsDeleted
        );
    }
}