using ErrorOr;
using OPS.Service.Dtos;

namespace OPS.Service.Contract;

public interface IExamService
{
    Task<ErrorOr<ExamDto>> GetExamAsync(long examId);
    Task<ErrorOr<ExamDto>> CreateExamAsync(ExamDto examDto);
}
