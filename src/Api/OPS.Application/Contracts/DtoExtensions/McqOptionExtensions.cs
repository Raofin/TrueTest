using ErrorOr;
using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.User;

namespace OPS.Application.Contracts.DtoExtensions;

public static class McqOptionExtensions
{
    public static McqOptionResponse ToDto(this McqOption mcqOption)
    {
        return new McqOptionResponse(
            mcqOption.Id,
            mcqOption.QuestionId,
            mcqOption.Option1,
            mcqOption.Option2,
            mcqOption.Option3,
            mcqOption.Option4,
            mcqOption.IsMultiSelect,
            mcqOption.AnswerOptions,
            mcqOption.CreatedAt,
            mcqOption.UpdatedAt
            );
    } 
} 


