using ErrorOr;
using FluentAssertions;
using NSubstitute;
using OPS.Application.Features.Exams.Queries;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Tests.Unit.Features.Exams.Queries;

public class GetExamByIdQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetExamByIdQueryHandler _sut;
    private readonly Examination _exam;
    private readonly Guid _validExamId;
    private readonly Guid _nonExistentExamId;

    public GetExamByIdQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetExamByIdQueryHandler(_unitOfWork);

        _validExamId = Guid.NewGuid();
        _nonExistentExamId = Guid.NewGuid();

        _exam = new Examination
        {
            Id = _validExamId,
            Title = "Test Exam",
            DescriptionMarkdown = "Test Description",
            DurationMinutes = 60,
            TotalPoints = 100,
            OpensAt = DateTime.UtcNow.AddDays(1),
            ClosesAt = DateTime.UtcNow.AddDays(2),
            IsPublished = false
        };
    }

    [Fact]
    public async Task Handle_WhenExamExists_ShouldReturnExamWithQuestions()
    {
        // Arrange
        _unitOfWork.Exam.GetWithQuestionsAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(_exam);

        var query = new GetExamByIdQuery(_validExamId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Exam.Should().BeEquivalentTo(_exam.MapToDto());
        result.Value.Questions.Should().BeEquivalentTo(_exam.MapToQuestionDto());
        await _unitOfWork.Exam.Received(1).GetWithQuestionsAsync(_validExamId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        _unitOfWork.Exam.GetWithQuestionsAsync(_nonExistentExamId, Arg.Any<CancellationToken>())
            .Returns((Examination?)null);

        var query = new GetExamByIdQuery(_nonExistentExamId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        await _unitOfWork.Exam.Received(1).GetWithQuestionsAsync(_nonExistentExamId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamHasNoQuestions_ShouldReturnExamWithEmptyQuestions()
    {
        // Arrange
        var examWithoutQuestions = new Examination
        {
            Id = _validExamId,
            Title = "Test Exam",
            DescriptionMarkdown = "Test Description",
            DurationMinutes = 60,
            TotalPoints = 100,
            OpensAt = DateTime.UtcNow.AddDays(1),
            ClosesAt = DateTime.UtcNow.AddDays(2),
            IsPublished = false,
            Questions = new List<Question>()
        };

        _unitOfWork.Exam.GetWithQuestionsAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(examWithoutQuestions);

        var query = new GetExamByIdQuery(_validExamId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Exam.Should().BeEquivalentTo(examWithoutQuestions.MapToDto());
        await _unitOfWork.Exam.Received(1).GetWithQuestionsAsync(_validExamId, Arg.Any<CancellationToken>());
    }
}