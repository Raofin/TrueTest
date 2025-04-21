using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Questions.ProblemSolving.Commands;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Questions.ProblemSolving.Commands;

public class DeleteProblemSolvingCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteProblemSolvingCommandHandler _sut;
    private readonly Question _question;
    private readonly Guid _validQuestionId;
    private readonly Guid _nonExistentQuestionId;
    private readonly DeleteProblemSolvingCommandValidator _validator = new();

    public DeleteProblemSolvingCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new DeleteProblemSolvingCommandHandler(_unitOfWork);

        _validQuestionId = Guid.NewGuid();
        _nonExistentQuestionId = Guid.NewGuid();

        _question = new Question
        {
            Id = _validQuestionId,
            ExaminationId = Guid.NewGuid(),
            QuestionTypeId = (int)QuestionType.ProblemSolving,
            Points = 20,
            Examination = new Examination
            {
                Id = Guid.NewGuid(),
                IsPublished = false,
                ProblemSolvingPoints = 20
            },
            TestCases = new List<TestCase>
            {
                new() { Id = Guid.NewGuid(), Input = "1 2 3", ExpectedOutput = "3" },
                new() { Id = Guid.NewGuid(), Input = "4 5 6", ExpectedOutput = "6" }
            }
        };

        // Set up default return values
        _unitOfWork.Question.GetWithTestCases(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Question)null!);
        _unitOfWork.Question.GetWithTestCases(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(_question);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);
    }

    [Fact]
    public async Task Handle_WhenQuestionExistsAndExamNotPublished_ShouldDeleteQuestion()
    {
        // Arrange
        var command = new DeleteProblemSolvingCommand(_validQuestionId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _unitOfWork.TestCase.Received(1).RemoveRange(_question.TestCases);
        _unitOfWork.Question.Received(1).Remove(_question);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _question.Examination.ProblemSolvingPoints.Should().Be(0);
    }

    [Fact]
    public async Task Handle_WhenQuestionDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteProblemSolvingCommand(_nonExistentQuestionId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        _unitOfWork.TestCase.DidNotReceive().RemoveRange(Arg.Any<List<TestCase>>());
        _unitOfWork.Question.DidNotReceive().Remove(Arg.Any<Question>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamIsPublished_ShouldReturnConflictError()
    {
        // Arrange
        _question.Examination.IsPublished = true;
        var command = new DeleteProblemSolvingCommand(_validQuestionId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Description.Should().Be("Exam of this question is already published");
        _unitOfWork.TestCase.DidNotReceive().RemoveRange(Arg.Any<List<TestCase>>());
        _unitOfWork.Question.DidNotReceive().Remove(Arg.Any<Question>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ShouldReturnUnexpectedError()
    {
        // Arrange
        var command = new DeleteProblemSolvingCommand(_validQuestionId);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        _unitOfWork.TestCase.Received(1).RemoveRange(_question.TestCases);
        _unitOfWork.Question.Received(1).Remove(_question);
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new DeleteProblemSolvingCommand(_validQuestionId);

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenQuestionIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteProblemSolvingCommand(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.QuestionId);
    }
}