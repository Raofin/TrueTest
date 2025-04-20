using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.Written.Queries;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Questions.Written.Queries;

public class GetAllWrittenByExamIdQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetAllWrittenByExamIdQueryHandler _sut;
    private readonly Guid _validExamId;
    private readonly Guid _nonExistentExamId;
    private readonly List<Question> _questions;
    private readonly GetAllWrittenByExamIdQueryValidator _validator = new();

    public GetAllWrittenByExamIdQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetAllWrittenByExamIdQueryHandler(_unitOfWork);

        _validExamId = Guid.NewGuid();
        _nonExistentExamId = Guid.NewGuid();

        _questions =
        [
            new Question
            {
                Id = Guid.NewGuid(),
                ExaminationId = _validExamId,
                QuestionTypeId = (int)QuestionType.Written,
                StatementMarkdown = "Explain the concept of Object-Oriented Programming in detail.",
                Points = 20,
                DifficultyId = (int)DifficultyType.Medium,
                HasLongAnswer = true
            },

            new Question
            {
                Id = Guid.NewGuid(),
                ExaminationId = _validExamId,
                QuestionTypeId = (int)QuestionType.Written,
                StatementMarkdown = "What are the SOLID principles? Explain each principle with examples.",
                Points = 25,
                DifficultyId = (int)DifficultyType.Hard,
                HasLongAnswer = true
            }
        ];

        // Set up default return values
        _unitOfWork.Question.GetWrittenByExamIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns([]);
        _unitOfWork.Question.GetWrittenByExamIdAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(_questions);
    }

    [Fact]
    public async Task Handle_WhenExamHasWrittenQuestions_ShouldReturnListOfQuestions()
    {
        // Arrange
        var query = new GetAllWrittenByExamIdQuery(_validExamId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(2);
        result.Value.Should().AllSatisfy(q =>
        {
            q.Should().BeOfType<WrittenQuestionResponse>();
            q.StatementMarkdown.Should().BeOneOf(_questions.Select(r => r.StatementMarkdown));
            q.Score.Should().BeOneOf(_questions.Select(r => r.Points));
            q.DifficultyType.Should().BeOneOf(_questions.Select(r => (DifficultyType)r.DifficultyId));
            q.HasLongAnswer.Should().BeTrue();
        });

        await _unitOfWork.Question.Received(1)
            .GetWrittenByExamIdAsync(_validExamId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamHasNoWrittenQuestions_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetAllWrittenByExamIdQuery(_nonExistentExamId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEmpty();
        await _unitOfWork.Question.Received(1)
            .GetWrittenByExamIdAsync(_nonExistentExamId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validator_WhenExamIdIsEmpty_ShouldReturnError()
    {
        // Arrange
        var query = new GetAllWrittenByExamIdQuery(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(query).ShouldHaveAnyValidationError();
    }
}