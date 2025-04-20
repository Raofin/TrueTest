using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.Mcq.Queries;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Questions.Mcq.Queries;

public class GetMcqByExamQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetMcqByExamQueryHandler _sut;
    private readonly Guid _validExamId;
    private readonly Guid _nonExistentExamId;
    private readonly List<Question> _questions;
    private readonly GetMcqByExamQueryValidator _validator = new();

    public GetMcqByExamQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetMcqByExamQueryHandler(_unitOfWork);

        _validExamId = Guid.NewGuid();
        _nonExistentExamId = Guid.NewGuid();

        _questions = new List<Question>
        {
            new Question
            {
                Id = Guid.NewGuid(),
                ExaminationId = _validExamId,
                QuestionTypeId = (int)QuestionType.MCQ,
                StatementMarkdown = "What is the capital of France?",
                Points = 10,
                DifficultyId = (int)DifficultyType.Easy,
                McqOption = new McqOption
                {
                    Id = Guid.NewGuid(),
                    Option1 = "Paris",
                    Option2 = "London",
                    Option3 = "Berlin",
                    Option4 = "Madrid",
                    IsMultiSelect = false,
                    AnswerOptions = "1"
                }
            },
            new Question
            {
                Id = Guid.NewGuid(),
                ExaminationId = _validExamId,
                QuestionTypeId = (int)QuestionType.MCQ,
                StatementMarkdown = "Which of these are programming languages?",
                Points = 15,
                DifficultyId = (int)DifficultyType.Medium,
                McqOption = new McqOption
                {
                    Id = Guid.NewGuid(),
                    Option1 = "C#",
                    Option2 = "Java",
                    Option3 = "Python",
                    Option4 = "HTML",
                    IsMultiSelect = true,
                    AnswerOptions = "1,2,3"
                }
            }
        };

        // Set up default return values
        _unitOfWork.Question.GetMcqByExamIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(new List<Question>());
        _unitOfWork.Question.GetMcqByExamIdAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(_questions);
    }

    [Fact]
    public async Task Handle_WhenExamHasMcqQuestions_ShouldReturnListOfMcqQuestions()
    {
        // Arrange
        var query = new GetMcqByExamQuery(_validExamId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(2);
        result.Value.Should().AllSatisfy(q =>
        {
            q.Should().BeOfType<McqQuestionResponse>();
            q.StatementMarkdown.Should().BeOneOf(_questions.Select(r => r.StatementMarkdown));
            q.Score.Should().BeOneOf(_questions.Select(r => r.Points));
            q.McqOption.Should().NotBeNull();
            q.McqOption.AnswerOptions.Should().BeOneOf(_questions.Select(r => r.McqOption!.AnswerOptions));
        });

        await _unitOfWork.Question.Received(1)
            .GetMcqByExamIdAsync(_validExamId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamHasNoMcqQuestions_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetMcqByExamQuery(_nonExistentExamId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEmpty();
        await _unitOfWork.Question.Received(1)
            .GetMcqByExamIdAsync(_nonExistentExamId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validator_WhenExamIdIsEmpty_ShouldReturnError()
    {
        // Arrange
        var query = new GetMcqByExamQuery(Guid.Empty);


        // Act & Assert
        _validator.TestValidate(query).ShouldHaveValidationErrorFor(x => x.ExamId);
    }
}