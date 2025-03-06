using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using OPS.Application.Contracts.Dtos;
using OPS.Application.Contracts.DtoExtensions;
using OPS.Domain.Entities.Exam;

using OPS.Domain.Enums;
using OPS.Application.Features.ExamQuestions.Queries;

namespace OPS.Application.Contracts.DtoExtensions;

public static class McqQuestionExtensions
{
    public static McqQuestionResponse OptionsToDto(this Question question,List<McqOptionResponse> options)
    {
        return new McqQuestionResponse(
            Id : question.Id,
            StatementMarkdown: question.StatementMarkdown,
            Score: question.Score,
            ExaminationId : question.ExaminationId,
            DifficultyId: question.DifficultyId,
            QuestionTypeId : question.QuestionTypeId,
            CreatedAt : question.CreatedAt,
            UpdatedAt : question.UpdatedAt,
            IsActive : question.IsActive,
            McqOptions : options
        );
    }
}