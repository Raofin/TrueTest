using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Candidates.Commands;
using OPS.Application.Interfaces.Auth;
using OPS.Domain;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Tests.Unit.Features.Candidates.Commands;

public class SubmitExamCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserProvider _userProvider;
    private readonly SubmitExamCommandHandler _sut;
    private readonly SubmitExamCommandValidator _validator = new();
    private readonly Guid _validExamId;
    private readonly Guid _validAccountId;

    public SubmitExamCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _userProvider = Substitute.For<IUserProvider>();
        _sut = new SubmitExamCommandHandler(_unitOfWork, _userProvider);

        _validExamId = Guid.NewGuid();
        _validAccountId = Guid.NewGuid();

        _userProvider.AccountId().Returns(_validAccountId);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldSubmitExam()
    {
        // Arrange
        var command = new SubmitExamCommand(_validExamId);
        var now = DateTime.UtcNow;

        var examCandidate = new ExamCandidate
        {
            AccountId = _validAccountId,
            ExaminationId = _validExamId,
            StartedAt = now.AddMinutes(-30),
            SubmittedAt = null
        };

        _unitOfWork.ExamCandidate.GetAsync(_validAccountId, _validExamId, Arg.Any<CancellationToken>())
            .Returns(examCandidate);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        examCandidate.SubmittedAt.Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamCandidateNotFound_ShouldReturnUnexpectedError()
    {
        // Arrange
        var command = new SubmitExamCommand(_validExamId);

        _unitOfWork.ExamCandidate.GetAsync(_validAccountId, _validExamId, Arg.Any<CancellationToken>())
            .Returns((ExamCandidate)null!);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ShouldReturnUnexpectedError()
    {
        // Arrange
        var command = new SubmitExamCommand(_validExamId);

        var examCandidate = new ExamCandidate
        {
            AccountId = _validAccountId,
            ExaminationId = _validExamId,
            StartedAt = DateTime.UtcNow.AddMinutes(-30),
            SubmittedAt = null
        };

        _unitOfWork.ExamCandidate.GetAsync(_validAccountId, _validExamId, Arg.Any<CancellationToken>())
            .Returns(examCandidate);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new SubmitExamCommand(_validExamId);

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new SubmitExamCommand(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("ExamId");
    }
}