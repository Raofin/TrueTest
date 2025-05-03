﻿using ErrorOr;
using FluentValidation;
using MediatR;
using OPS.Application.Common;
using OPS.Application.Dtos;
using OPS.Application.Mappers;
using OPS.Domain;

namespace OPS.Application.Features.Exams.Commands;

public record UpdateExamCommand(
    Guid ExamId,
    string? Title,
    string? Description,
    int? DurationMinutes,
    decimal? TotalPoints,
    DateTime? OpensAt,
    DateTime? ClosesAt) : IRequest<ErrorOr<ExamResponse>>;

public class UpdateExamCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateExamCommand, ErrorOr<ExamResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ExamResponse>> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
    {
        var exam = await _unitOfWork.Exam.GetAsync(request.ExamId, cancellationToken);
        if (exam is null) return Error.NotFound();
        if (exam.IsPublished) return Error.Conflict(description: "Exam is already published");

        exam.Title = request.Title ?? exam.Title;
        exam.DescriptionMarkdown = request.Description ?? exam.DescriptionMarkdown;
        exam.DurationMinutes = request.DurationMinutes ?? exam.DurationMinutes;
        exam.TotalPoints = request.TotalPoints ?? exam.TotalPoints;
        exam.OpensAt = request.OpensAt ?? exam.OpensAt;
        exam.ClosesAt = request.ClosesAt ?? exam.ClosesAt;

        await _unitOfWork.CommitAsync(cancellationToken);

        return exam.MapToDto();
    }
}

public class UpdateExamCommandValidator : AbstractValidator<UpdateExamCommand>
{
    public UpdateExamCommandValidator()
    {
        RuleFor(x => x.ExamId)
            .IsValidGuid();

        RuleFor(x => x.Title)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Description)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(10)
            .When(x => x.DurationMinutes.HasValue);

        RuleFor(x => x.OpensAt)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.OpensAt.HasValue);

        RuleFor(x => x.ClosesAt)
            .GreaterThan(x => x.OpensAt)
            .When(x => x.ClosesAt.HasValue && x.OpensAt.HasValue);
    }
}