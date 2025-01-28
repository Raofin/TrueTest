using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using OPS.Service.Contract;
using OPS.Service.Dtos;

namespace OPS.Api.Controllers;
public class ExamController(IExamService examService) : ApiController
{
    private readonly IExamService _examService = examService;

    [HttpGet("{examId}")]
    public async Task<IActionResult> GetExamAsync(long examId)
    {
        var dto = await _examService.GetExamAsync(examId);

        return !dto.IsError ? Ok(dto.Value) : dto.FirstError.Type switch
        {
            ErrorType.NotFound => NotFound("Exam was not found."),
            _ => Problem("An unexpected error occurred.")
        };
    }

    [HttpPost()]
    public async Task<IActionResult> CreateExamAsync(ExamDto examDto)
    {
        var dto = await _examService.CreateExamAsync(examDto);

        return !dto.IsError
            ? Ok(dto.Value)
            : Problem(dto.FirstError.Description);
    }
}
