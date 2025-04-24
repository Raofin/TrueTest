using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Review.Commands;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Tests.Unit.Features.Review.Commands;

public class ReviewProblemCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ReviewProblemCommandHandler _sut;
    private readonly ReviewProblemCommandValidator _validator = new();
    private readonly Guid _validExamId;
    private readonly Guid _validAccountId;
    private readonly Guid _validSubmissionId;

    public ReviewProblemCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new ReviewProblemCommandHandler(_unitOfWork);

        _validExamId = Guid.NewGuid();
        _validAccountId = Guid.NewGuid();
        _validSubmissionId = Guid.NewGuid();
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldUpdateScoreAndFlag()
    {
        // Arrange
        var command = new ReviewProblemCommand(
            _validExamId,
            _validAccountId,
            _validSubmissionId,
            85,
            true,
            "Suspicious code pattern");

        var submission = new ProblemSubmission
        {
            Id = _validSubmissionId,
            Score = 70,
            IsFlagged = false,
            FlagReason = null
        };

        var candidate = new ExamCandidate
        {
            AccountId = _validAccountId,
            ExaminationId = _validExamId,
            ProblemSolvingScore = 70
        };

        _unitOfWork.ProblemSubmission.GetAsync(_validSubmissionId, Arg.Any<CancellationToken>())
            .Returns(submission);

        _unitOfWork.ExamCandidate.GetAsync(_validAccountId, _validExamId, Arg.Any<CancellationToken>())
            .Returns(candidate);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        submission.Score.Should().Be(85);
        submission.IsFlagged.Should().BeTrue();
        submission.FlagReason.Should().Be("Suspicious code pattern");
        candidate.ProblemSolvingScore.Should().Be(85);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenSubmissionNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new ReviewProblemCommand(
            _validExamId,
            _validAccountId,
            _validSubmissionId,
            85,
            true,
            "Suspicious code pattern");

        _unitOfWork.ProblemSubmission.GetAsync(_validSubmissionId, Arg.Any<CancellationToken>())
            .Returns((ProblemSubmission)null!);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCandidateNotFound_ShouldReturnUnexpectedError()
    {
        // Arrange
        var command = new ReviewProblemCommand(
            _validExamId,
            _validAccountId,
            _validSubmissionId,
            85,
            true,
            "Suspicious code pattern");

        var submission = new ProblemSubmission
        {
            Id = _validSubmissionId,
            Score = 70,
            IsFlagged = false,
            FlagReason = null
        };

        _unitOfWork.ProblemSubmission.GetAsync(_validSubmissionId, Arg.Any<CancellationToken>())
            .Returns(submission);

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
    public async Task Handle_WhenOnlyFlagging_ShouldUpdateFlagOnly()
    {
        // Arrange
        var command = new ReviewProblemCommand(
            _validExamId,
            _validAccountId,
            _validSubmissionId,
            null,
            true,
            "Suspicious code pattern");

        var submission = new ProblemSubmission
        {
            Id = _validSubmissionId,
            Score = 70,
            IsFlagged = false,
            FlagReason = null
        };

        var candidate = new ExamCandidate
        {
            AccountId = _validAccountId,
            ExaminationId = _validExamId,
            ProblemSolvingScore = 70
        };

        _unitOfWork.ProblemSubmission.GetAsync(_validSubmissionId, Arg.Any<CancellationToken>())
            .Returns(submission);

        _unitOfWork.ExamCandidate.GetAsync(_validAccountId, _validExamId, Arg.Any<CancellationToken>())
            .Returns(candidate);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        submission.Score.Should().Be(70);
        submission.IsFlagged.Should().BeTrue();
        submission.FlagReason.Should().Be("Suspicious code pattern");
        candidate.ProblemSolvingScore.Should().Be(70);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new ReviewProblemCommand(
            _validExamId,
            _validAccountId,
            _validSubmissionId,
            85,
            true,
            "Suspicious code pattern");

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenSubmissionIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ReviewProblemCommand(
            _validExamId,
            _validAccountId,
            Guid.Empty,
            85,
            true,
            "Suspicious code pattern");

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("ProblemSubmissionId");
    }

    [Fact]
    public void Validate_WhenScoreIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ReviewProblemCommand(
            _validExamId,
            _validAccountId,
            _validSubmissionId,
            150,
            true,
            "Suspicious code pattern");

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("Score");
    }

    [Fact]
    public void Validate_WhenFlagReasonIsTooLong_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ReviewProblemCommand(
            _validExamId,
            _validAccountId,
            _validSubmissionId,
            85,
            true,
            new string('a', 501));

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("FlagReason");
    }
}