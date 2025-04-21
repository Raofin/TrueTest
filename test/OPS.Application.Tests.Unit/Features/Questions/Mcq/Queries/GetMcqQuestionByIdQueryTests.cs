using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.Mcq.Queries;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Questions.Mcq.Queries;

public class GetMcqQuestionByIdQueryTests
{
    private readonly GetMcqQuestionByIdQueryHandler _sut;
    private readonly Question _question;
    private readonly Guid _validQuestionId;
    private readonly Guid _nonExistentQuestionId;
    private readonly GetMcqQuestionByIdQueryValidator _validator = new();

    public GetMcqQuestionByIdQueryTests()
    {
        IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetMcqQuestionByIdQueryHandler(unitOfWork);

        _validQuestionId = Guid.NewGuid();
        _nonExistentQuestionId = Guid.NewGuid();

        _question = new Question
        {
            Id = _validQuestionId,
            StatementMarkdown = "Test question",
            Points = 10,
            DifficultyId = (int)DifficultyType.Easy,
            QuestionTypeId = (int)QuestionType.MCQ,
            McqOption = new McqOption
            {
                Option1 = "Option 1",
                Option2 = "Option 2",
                Option3 = "Option 3",
                Option4 = "Option 4",
                IsMultiSelect = false,
                AnswerOptions = "1"
            }
        };

        // Set up default return values
        unitOfWork.Question.GetWithMcqOption(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Question)null!);
        unitOfWork.Question.GetWithMcqOption(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(_question);
    }

    [Fact]
    public async Task Handle_WhenQuestionExists_ShouldReturnQuestion()
    {
        // Arrange
        var query = new GetMcqQuestionByIdQuery(_validQuestionId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<McqQuestionResponse>();
        result.Value.StatementMarkdown.Should().Be(_question.StatementMarkdown);
        result.Value.Score.Should().Be(_question.Points);
        result.Value.DifficultyType.Should().Be((DifficultyType)_question.DifficultyId);
        result.Value.McqOption.Option1.Should().Be(_question.McqOption!.Option1);
        result.Value.McqOption.Option2.Should().Be(_question.McqOption.Option2);
        result.Value.McqOption.Option3.Should().Be(_question.McqOption.Option3);
        result.Value.McqOption.Option4.Should().Be(_question.McqOption.Option4);
        result.Value.McqOption.IsMultiSelect.Should().Be(_question.McqOption.IsMultiSelect);
        result.Value.McqOption.AnswerOptions.Should().Be(_question.McqOption.AnswerOptions);
    }

    [Fact]
    public async Task Handle_WhenQuestionDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = new GetMcqQuestionByIdQuery(_nonExistentQuestionId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public void Handle_WhenQuestionIdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var query = new GetMcqQuestionByIdQuery(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(query).ShouldHaveValidationErrorFor(x => x.QuestionId);
    }
}