using ErrorOr;
using FluentAssertions;
using NSubstitute;
using OPS.Application.Features.Exams.Commands;
using OPS.Domain;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Tests.Unit.Features.Exams.Commands;

public class CreateExamCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateExamCommandHandler _sut;
    private readonly DateTime _opensAt = DateTime.UtcNow.AddDays(1);
    private readonly DateTime _closesAt = DateTime.UtcNow.AddDays(2);

    public CreateExamCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new CreateExamCommandHandler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_WhenValidCommand_ShouldCreateExamAndReturnSuccess()
    {
        // Arrange
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        var command = new CreateExamCommand(
            "Test Exam",
            "Test Description",
            60,
            100,
            _opensAt,
            _closesAt);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be(command.Title);
        result.Value.Description.Should().Be(command.Description);
        result.Value.DurationMinutes.Should().Be(command.DurationMinutes);
        result.Value.TotalPoints.Should().Be(command.TotalPoints);
        result.Value.OpensAt.Should().Be(command.OpensAt);
        result.Value.ClosesAt.Should().Be(command.ClosesAt);
        _unitOfWork.Exam.Received(1).Add(Arg.Is<Examination>(exam =>
            exam.Title == command.Title &&
            exam.DescriptionMarkdown == command.Description &&
            exam.DurationMinutes == command.DurationMinutes &&
            exam.TotalPoints == command.TotalPoints &&
            exam.OpensAt == command.OpensAt &&
            exam.ClosesAt == command.ClosesAt));
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ShouldReturnUnexpectedError()
    {
        // Arrange
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        var command = new CreateExamCommand(
            "Test Exam",
            "Test Description",
            60,
            100,
            _opensAt,
            _closesAt);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        _unitOfWork.Exam.Received(1).Add(Arg.Any<Examination>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("", "Description", 60, 100)]
    [InlineData("Title", "", 60, 100)]
    [InlineData("Title", "Description", 0, 100)]
    [InlineData("Title", "Description", -1, 100)]
    [InlineData("Title", "Description", 60, 100)]
    public void Validate_WhenInvalidCommand_ShouldReturnValidationError(
        string title,
        string description,
        int durationMinutes,
        decimal totalPoints)
    {
        // Arrange
        var command = new CreateExamCommand(
            title,
            description,
            durationMinutes,
            totalPoints,
            _closesAt, // Invalid: ClosesAt before OpensAt
            _opensAt);

        var validator = new CreateExamCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }


    [Fact]
    public void Validate_WhenValidCommand_ShouldReturnSuccess()
    {
        // Arrange
        var command = new CreateExamCommand(
            "Test Exam",
            "Test Description",
            60,
            100,
            _opensAt,
            _closesAt);

        var validator = new CreateExamCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}