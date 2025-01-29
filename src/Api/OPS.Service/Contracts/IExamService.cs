using ErrorOr;
using OPS.Service.Dtos;

namespace OPS.Service.Contracts;

public interface IExamService
{
    Task<ErrorOr<List<ExamDto>>> GetAsync();
    Task<ErrorOr<ExamDto>> GetByIdAsync(long examId);
    Task<ErrorOr<ExamDto>> CreateAsync(ExamCreateDto dto);
    Task<ErrorOr<ExamDto>> UpdateAsync(ExamUpdateDto dto);
    Task<ErrorOr<Success>> DeleteAsync(long examId);
    Task<ErrorOr<List<ExamDto>>> GetUpcomingExamsAsync();
}