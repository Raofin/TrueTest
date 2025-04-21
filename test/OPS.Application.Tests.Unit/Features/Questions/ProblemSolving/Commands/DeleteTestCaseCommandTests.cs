using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Questions.ProblemSolving.Commands;
using OPS.Domain;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Tests.Unit.Features.Questions.ProblemSolving.Commands;

public class DeleteTestCaseCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteTestCaseCommandHandler _sut;
    private readonly TestCase _testCase;
    private readonly Guid _validTestCaseId;
    private readonly Guid _nonExistentTestCaseId;
    private readonly DeleteTestCaseCommandValidator _validator = new();

    public DeleteTestCaseCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new DeleteTestCaseCommandHandler(_unitOfWork);

        _validTestCaseId = Guid.NewGuid();
        _nonExistentTestCaseId = Guid.NewGuid();

        _testCase = new TestCase
        {
            Id = _validTestCaseId,
            QuestionId = Guid.NewGuid(),
            Input = "1 2 3",
            ExpectedOutput = "3"
        };

        // Set up default return values
        _unitOfWork.TestCase.GetAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((TestCase)null!);
        _unitOfWork.TestCase.GetAsync(_validTestCaseId, Arg.Any<CancellationToken>())
            .Returns(_testCase);
        _unitOfWork.Exam.IsPublished(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(false);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);
    }

    [Fact]
    public async Task Handle_WhenTestCaseExistsAndExamNotPublished_ShouldDeleteTestCase()
    {
        // Arrange
        var command = new DeleteTestCaseCommand(_validTestCaseId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _unitOfWork.TestCase.Received(1).Remove(_testCase);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenTestCaseDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteTestCaseCommand(_nonExistentTestCaseId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        _unitOfWork.TestCase.DidNotReceive().Remove(Arg.Any<TestCase>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamIsPublished_ShouldReturnConflictError()
    {
        // Arrange
        _unitOfWork.Exam.IsPublished(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(true);
        var command = new DeleteTestCaseCommand(_validTestCaseId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Description.Should().Be("Exam of this question is already published");
        _unitOfWork.TestCase.DidNotReceive().Remove(Arg.Any<TestCase>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ShouldReturnUnexpectedError()
    {
        // Arrange
        var command = new DeleteTestCaseCommand(_validTestCaseId);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        _unitOfWork.TestCase.Received(1).Remove(_testCase);
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new DeleteTestCaseCommand(_validTestCaseId);

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenTestCaseIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteTestCaseCommand(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.TestCaseId);
    }
}