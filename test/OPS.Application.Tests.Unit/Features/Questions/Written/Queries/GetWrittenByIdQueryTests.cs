using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.Written.Queries;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Questions.Written.Queries;

public class GetWrittenByIdQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetWrittenByIdQueryHandler _sut;
    private readonly Question _question;
    private readonly Guid _validQuestionId;
    private readonly Guid _nonExistentQuestionId;
    private readonly GetWrittenByIdQueryValidator _validator = new();

    public GetWrittenByIdQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetWrittenByIdQueryHandler(_unitOfWork);

        _validQuestionId = Guid.NewGuid();
        _nonExistentQuestionId = Guid.NewGuid();

        _question = new Question
        {
            Id = _validQuestionId,
            ExaminationId = Guid.NewGuid(),
            QuestionTypeId = (int)QuestionType.Written,
            StatementMarkdown = "Explain the concept of Object-Oriented Programming in detail.",
            Points = 20,
            DifficultyId = (int)DifficultyType.Medium,
            HasLongAnswer = true
        };

        // Set up default return values
        _unitOfWork.Question.GetWrittenByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Question)null!);
        _unitOfWork.Question.GetWrittenByIdAsync(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(_question);
    }

    [Fact]
    public async Task Handle_WhenQuestionExists_ShouldReturnQuestion()
    {
        // Arrange
        var query = new GetWrittenByIdQuery(_validQuestionId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<WrittenQuestionResponse>();
        result.Value.StatementMarkdown.Should().Be(_question.StatementMarkdown);
        result.Value.Score.Should().Be(_question.Points);
        result.Value.DifficultyType.Should().Be((DifficultyType)_question.DifficultyId);
        result.Value.HasLongAnswer.Should().Be(_question.HasLongAnswer);

        await _unitOfWork.Question.Received(1)
            .GetWrittenByIdAsync(_validQuestionId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenQuestionDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = new GetWrittenByIdQuery(_nonExistentQuestionId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        await _unitOfWork.Question.Received(1)
            .GetWrittenByIdAsync(_nonExistentQuestionId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Handle_WhenQuestionIdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var query = new GetWrittenByIdQuery(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(query).ShouldHaveAnyValidationError();
    }
}