using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Candidates.Commands;
using OPS.Application.Interfaces.Auth;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;

namespace OPS.Application.Tests.Unit.Features.Candidates.Commands;

public class SaveMcqSubmissionCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserProvider _userProvider;
    private readonly SaveMcqSubmissionsCommandHandler _sut;
    private readonly SaveMcqSubmissionsCommandValidator _validator = new();
    private readonly Guid _validExamId;
    private readonly Guid _validQuestionId;
    private readonly Guid _validAccountId;
    private readonly Guid _validMcqOptionId;

    public SaveMcqSubmissionCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _userProvider = Substitute.For<IUserProvider>();
        _sut = new SaveMcqSubmissionsCommandHandler(_unitOfWork, _userProvider);

        _validExamId = Guid.NewGuid();
        _validQuestionId = Guid.NewGuid();
        _validAccountId = Guid.NewGuid();
        _validMcqOptionId = Guid.NewGuid();

        _userProvider.AccountId().Returns(_validAccountId);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldSaveSubmissions()
    {
        // Arrange
        var submissions = new List<McqSubmissionRequest>
        {
            new(_validQuestionId, "1,2")
        };

        var command = new SaveMcqSubmissionsCommand(_validExamId, submissions);

        _unitOfWork.ExamCandidate.IsValidCandidate(_validAccountId, _validExamId, Arg.Any<CancellationToken>())
            .Returns(true);

        var question = new Question
        {
            Id = _validQuestionId,
            Points = 10,
            McqOption = new McqOption
            {
                Id = _validMcqOptionId,
                AnswerOptions = "1,2"
            }
        };

        _unitOfWork.Question.GetWithMcqOption(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(question);

        _unitOfWork.McqSubmission.GetByAccountIdAsync(_validQuestionId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns((McqSubmission)null!);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _unitOfWork.McqSubmission.Received(1).Add(Arg.Is<McqSubmission>(s =>
            s.QuestionId == _validQuestionId &&
            s.AccountId == _validAccountId &&
            s.McqOptionId == _validMcqOptionId &&
            s.AnswerOptions == "1,2" &&
            s.Score == 10));
    }

    [Fact]
    public async Task Handle_WhenNotValidCandidate_ShouldReturnForbiddenError()
    {
        // Arrange
        var submissions = new List<McqSubmissionRequest>
        {
            new(_validQuestionId, "1,2")
        };

        var command = new SaveMcqSubmissionsCommand(_validExamId, submissions);

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
        var submissions = new List<McqSubmissionRequest>
        {
            new(_validQuestionId, "1,2,3")
        };

        var command = new SaveMcqSubmissionsCommand(_validExamId, submissions);

        _unitOfWork.ExamCandidate.IsValidCandidate(_validAccountId, _validExamId, Arg.Any<CancellationToken>())
            .Returns(true);

        var question = new Question
        {
            Id = _validQuestionId,
            Points = 10,
            McqOption = new McqOption
            {
                Id = _validMcqOptionId,
                AnswerOptions = "1,2,3"
            }
        };

        var existingSubmission = new McqSubmission
        {
            QuestionId = _validQuestionId,
            AccountId = _validAccountId,
            McqOptionId = _validMcqOptionId,
            AnswerOptions = "1,2",
            Score = 0,
            Question = question,
            McqOption = question.McqOption
        };

        _unitOfWork.McqSubmission.GetByAccountIdAsync(_validQuestionId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(existingSubmission);

        _unitOfWork.Question.GetWithMcqOption(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(new Question
                {
                    Id = _validQuestionId,
                    Points = 10,
                    McqOption = new McqOption { AnswerOptions = "1,2,3" }
                }
            );

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        existingSubmission.AnswerOptions.Should().Be("1,2,3");
        existingSubmission.Score.Should().Be(10);
    }

    [Fact]
    public async Task Handle_WhenAnswerIsIncorrect_ShouldSetScoreToZero()
    {
        // Arrange
        var submissions = new List<McqSubmissionRequest>
        {
            new(_validQuestionId, "1,2,3")
        };

        var command = new SaveMcqSubmissionsCommand(_validExamId, submissions);

        _unitOfWork.ExamCandidate.IsValidCandidate(_validAccountId, _validExamId, Arg.Any<CancellationToken>())
            .Returns(true);

        var question = new Question
        {
            Id = _validQuestionId,
            Points = 10,
            McqOption = new McqOption
            {
                Id = _validMcqOptionId,
                AnswerOptions = "1,3"
            }
        };

        _unitOfWork.Question.GetWithMcqOption(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(question);

        _unitOfWork.McqSubmission.GetByAccountIdAsync(_validQuestionId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns((McqSubmission)null!);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _unitOfWork.McqSubmission.Received(1).Add(Arg.Is<McqSubmission>(s =>
            s.QuestionId == _validQuestionId &&
            s.AccountId == _validAccountId &&
            s.McqOptionId == _validMcqOptionId &&
            s.AnswerOptions == "1,2,3" &&
            s.Score == 0));
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var submissions = new List<McqSubmissionRequest>
        {
            new(_validQuestionId, "1,2")
        };

        var command = new SaveMcqSubmissionsCommand(_validExamId, submissions);

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenQuestionIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var submissions = new List<McqSubmissionRequest>
        {
            new(Guid.Empty, "1,2")
        };

        var command = new SaveMcqSubmissionsCommand(_validExamId, submissions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("Submissions[0].QuestionId");
    }

    [Fact]
    public void Validate_WhenAnswerOptionsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var submissions = new List<McqSubmissionRequest>
        {
            new(_validQuestionId, "")
        };

        var command = new SaveMcqSubmissionsCommand(_validExamId, submissions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("Submissions[0].CandidateAnswerOptions");
    }

    [Fact]
    public void Validate_WhenAnswerOptionsIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var submissions = new List<McqSubmissionRequest>
        {
            new(_validQuestionId, "1,5") // 5 is invalid
        };

        var command = new SaveMcqSubmissionsCommand(_validExamId, submissions);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("Submissions[0].CandidateAnswerOptions");
    }
}