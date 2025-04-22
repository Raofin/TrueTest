using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Candidates.Commands;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Candidates.Commands;

public class StartExamCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider;
    private readonly StartExamCommandHandler _sut;
    private readonly StartExamCommandValidator _validator = new();
    private readonly Guid _validExamId;
    private readonly Guid _validAccountId;
    private readonly Guid _validQuestionId;
    private readonly Guid _validSubmissionId;

    public StartExamCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _userInfoProvider = Substitute.For<IUserInfoProvider>();
        _sut = new StartExamCommandHandler(_unitOfWork, _userInfoProvider);

        _validExamId = Guid.NewGuid();
        _validAccountId = Guid.NewGuid();
        _validQuestionId = Guid.NewGuid();
        _validSubmissionId = Guid.NewGuid();

        _userInfoProvider.AccountId().Returns(_validAccountId);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldStartExamAndReturnResponse()
    {
        // Arrange
        var command = new StartExamCommand(_validExamId);
        var now = DateTime.UtcNow;
        var examDuration = 60;

        var candidate = new ExamCandidate
        {
            AccountId = _validAccountId,
            StartedAt = null,
            SubmittedAt = DateTime.MaxValue
        };

        var exam = new Examination()
        {
            Id = _validExamId,
            DurationMinutes = examDuration,
            ClosesAt = now.AddHours(2),
            Questions = new List<Question>
            {
                new()
                {
                    Id = _validQuestionId,
                    ProblemSubmissions = new List<ProblemSubmission>
                    {
                        new()
                        {
                            Id = _validSubmissionId,
                            Code = "print('Hello')",
                            LanguageId = LanguageId.python.ToString()
                        }
                    }
                }
            }
        };

        _unitOfWork.Exam.GetCandidateAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(candidate);

        _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(exam);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();

        candidate.StartedAt.Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
        candidate.SubmittedAt.Should().BeCloseTo(now.AddMinutes(examDuration), TimeSpan.FromSeconds(1));
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCandidateNotInvited_ShouldReturnUnauthorizedError()
    {
        // Arrange
        var command = new StartExamCommand(_validExamId);

        _unitOfWork.Exam.GetCandidateAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns((ExamCandidate)null!);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Forbidden);
        result.FirstError.Description.Should().Be("Candidate was not invited to this exam");
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamAlreadyEnded_ShouldReturnUnauthorizedError()
    {
        // Arrange
        var command = new StartExamCommand(_validExamId);

        var candidate = new ExamCandidate
        {
            AccountId = _validAccountId,
            StartedAt = null,
            SubmittedAt = DateTime.UtcNow.AddHours(-1)
        };

        _unitOfWork.Exam.GetCandidateAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(candidate);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Forbidden);
        result.FirstError.Description.Should().Be("Exam is already submitted or ended");
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamNotFound_ShouldReturnUnexpectedError()
    {
        // Arrange
        var command = new StartExamCommand(_validExamId);

        var candidate = new ExamCandidate
        {
            AccountId = _validAccountId,
            StartedAt = null,
            SubmittedAt = DateTime.MaxValue
        };

        _unitOfWork.Exam.GetCandidateAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(candidate);

        _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns((Examination)null!);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        result.FirstError.Description.Should().Be("Invalid exam");
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamAlreadyStarted_ShouldReturnExistingResponse()
    {
        // Arrange
        var command = new StartExamCommand(_validExamId);
        var startedAt = DateTime.UtcNow.AddMinutes(-30);
        var examDuration = 60;

        var candidate = new ExamCandidate
        {
            AccountId = _validAccountId,
            StartedAt = startedAt,
            SubmittedAt = startedAt.AddMinutes(examDuration)
        };

        var exam = new Examination()
        {
            Id = _validExamId,
            DurationMinutes = examDuration,
            ClosesAt = DateTime.UtcNow.AddHours(2),
            Questions = new List<Question>()
        };

        _unitOfWork.Exam.GetCandidateAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(candidate);

        _unitOfWork.Exam.GetWithQuesAndSubmissionsAsync(_validExamId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(exam);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();

        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new StartExamCommand(_validExamId);

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new StartExamCommand(Guid.Empty);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("ExamId");
    }
}