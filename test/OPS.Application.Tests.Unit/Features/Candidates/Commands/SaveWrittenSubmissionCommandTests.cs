using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Candidates.Commands;
using OPS.Application.Interfaces.Auth;
using OPS.Domain;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Tests.Unit.Features.Candidates.Commands;

public class SaveWrittenSubmissionCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SaveWrittenSubmissionsCommandHandler _sut;
    private readonly SaveWrittenSubmissionsCommandValidator _validator = new();
    private readonly Guid _validExamId;
    private readonly Guid _validQuestionId;
    private readonly Guid _validAccountId;

    public SaveWrittenSubmissionCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        var userInfoProvider = Substitute.For<IUserProvider>();
        _sut = new SaveWrittenSubmissionsCommandHandler(_unitOfWork, userInfoProvider);

        _validExamId = Guid.NewGuid();
        _validQuestionId = Guid.NewGuid();
        _validAccountId = Guid.NewGuid();

        userInfoProvider.AccountId().Returns(_validAccountId);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldSaveSubmissions()
    {
        // Arrange
        var submissions = new List<WrittenSubmissionRequest>
        {
            new(_validQuestionId, "Test answer")
        };

        var command = new SaveWrittenSubmissionsCommand(_validExamId, submissions);

        _unitOfWork.ExamCandidate.IsValidCandidate(_validAccountId, _validExamId, Arg.Any<CancellationToken>())
            .Returns(true);

        _unitOfWork.WrittenSubmission.GetByAccountIdAsync(_validQuestionId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns((WrittenSubmission)null!);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _unitOfWork.WrittenSubmission.Received(1).Add(Arg.Is<WrittenSubmission>(s =>
            s.QuestionId == _validQuestionId &&
            s.AccountId == _validAccountId &&
            s.Answer == "Test answer"));
    }

    [Fact]
    public async Task Handle_WhenNotValidCandidate_ShouldReturnForbiddenError()
    {
        // Arrange
        var submissions = new List<WrittenSubmissionRequest>
        {
            new(_validQuestionId, "Test answer")
        };

        var command = new SaveWrittenSubmissionsCommand(_validExamId, submissions);

        _unitOfWork.ExamCandidate.IsValidCandidate(_validAccountId, _validExamId, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Forbidden);
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUpdatingExistingSubmission_ShouldUpdateSubmission()
    {
        // Arrange
        var submissions = new List<WrittenSubmissionRequest>
        {
            new(_validQuestionId, "Updated answer")
        };

        var command = new SaveWrittenSubmissionsCommand(_validExamId, submissions);

        _unitOfWork.ExamCandidate.IsValidCandidate(_validAccountId, _validExamId, Arg.Any<CancellationToken>())
            .Returns(true);

        var existingSubmission = new WrittenSubmission
        {
            QuestionId = _validQuestionId,
            AccountId = _validAccountId,
            Answer = "Original answer"
        };

        _unitOfWork.WrittenSubmission.GetByAccountIdAsync(_validQuestionId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(existingSubmission);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        existingSubmission.Answer.Should().Be("Updated answer");
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var submissions = new List<WrittenSubmissionRequest>
        {
            new(_validQuestionId, "Test answer")
        };

        var command = new SaveWrittenSubmissionsCommand(_validExamId, submissions);

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenQuestionIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var submissions = new List<WrittenSubmissionRequest>
        {
            new(Guid.Empty, "Test answer")
        };

        var command = new SaveWrittenSubmissionsCommand(_validExamId, submissions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("Submissions[0].QuestionId");
    }

    [Fact]
    public void Validate_WhenAnswerIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var submissions = new List<WrittenSubmissionRequest>
        {
            new(_validQuestionId, "")
        };

        var command = new SaveWrittenSubmissionsCommand(_validExamId, submissions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("Submissions[0].CandidateAnswer");
    }
}