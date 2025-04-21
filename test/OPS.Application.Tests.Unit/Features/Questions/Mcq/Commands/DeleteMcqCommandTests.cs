using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Questions.Mcq.Commands;
using OPS.Domain;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Tests.Unit.Features.Questions.Mcq.Commands;

public class DeleteMcqCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteMcqQuestionCommandHandler _sut;
    private readonly Question _question;
    private readonly Guid _validQuestionId;
    private readonly Guid _nonExistentQuestionId;
    private readonly DeleteMcqCommandValidator _validator = new();

    public DeleteMcqCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new DeleteMcqQuestionCommandHandler(_unitOfWork);

        _validQuestionId = Guid.NewGuid();
        _nonExistentQuestionId = Guid.NewGuid();

        _question = new Question
        {
            Id = _validQuestionId,
            Points = 10,
            McqOption = new McqOption(),
            Examination = new Examination
            {
                IsPublished = false,
                McqPoints = 10
            }
        };

        // Set up default return values
        _unitOfWork.Question.GetWithMcqOption(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Question)null!);
        _unitOfWork.Question.GetWithMcqOption(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(_question);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);
    }

    [Fact]
    public async Task Handle_WhenQuestionExistsAndExamNotPublished_ShouldDeleteQuestion()
    {
        // Arrange
        var command = new DeleteMcqCommand(_validQuestionId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _unitOfWork.McqOption.Received(1).Remove(_question.McqOption!);
        _unitOfWork.Question.Received(1).Remove(_question);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _question.Examination.McqPoints.Should().Be(0);
    }

    [Fact]
    public async Task Handle_WhenQuestionDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteMcqCommand(_nonExistentQuestionId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        _unitOfWork.McqOption.DidNotReceive().Remove(Arg.Any<McqOption>());
        _unitOfWork.Question.DidNotReceive().Remove(Arg.Any<Question>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamIsPublished_ShouldReturnConflictError()
    {
        // Arrange
        _question.Examination.IsPublished = true;
        var command = new DeleteMcqCommand(_validQuestionId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        _unitOfWork.McqOption.DidNotReceive().Remove(Arg.Any<McqOption>());
        _unitOfWork.Question.DidNotReceive().Remove(Arg.Any<Question>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ShouldReturnUnexpectedError()
    {
        // Arrange
        var command = new DeleteMcqCommand(_validQuestionId);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        _unitOfWork.McqOption.Received(1).Remove(_question.McqOption!);
        _unitOfWork.Question.Received(1).Remove(_question);
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new DeleteMcqCommand(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.QuestionId);
    }
}