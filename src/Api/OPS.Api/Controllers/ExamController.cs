using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using OPS.Service.Contracts;
using OPS.Service.Dtos;

namespace OPS.Api.Controllers;

public class ExamController(IExamService examService) : ApiController
{
    private readonly IExamService _examService = examService;

    [HttpGet]
    public async Task<IActionResult> GetAsync(long? examId)
    {
        dynamic result = examId.HasValue
            ? await _examService.GetByIdAsync(examId.Value)
            : await _examService.GetAsync();

        return !result.IsError
            ? Ok(result.Value)
            : result.FirstError.Type switch
            {
                ErrorType.NotFound => NotFound("Exam was not found."),
                _ => Problem("An unexpected error occurred.")
            };
    }

    [HttpGet("UpcomingExams")]
    public async Task<IActionResult> GetUpcomingExamsAsync()
    {
        var result = await _examService.GetUpcomingExamsAsync();

        return !result.IsError
            ? Ok(result.Value)
            : Problem(result.FirstError.Description);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(ExamCreateDto examDto)
    {
        var dto = await _examService.CreateAsync(examDto);

        return !dto.IsError
            ? Ok(dto.Value)
            : Problem(dto.FirstError.Description);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(ExamUpdateDto examDto)
    {
        var dto = await _examService.UpdateAsync(examDto);

        return !dto.IsError
            ? Ok(dto.Value)
            : Problem(dto.FirstError.Description);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(long examId)
    {
        var result = await _examService.DeleteAsync(examId);

        return !result.IsError
            ? Ok("Exam was deleted.")
            : result.FirstError.Type switch
            {
                ErrorType.NotFound => NotFound("Exam was not found."),
                _ => Problem("An unexpected error occurred.")
            };
    }
}
