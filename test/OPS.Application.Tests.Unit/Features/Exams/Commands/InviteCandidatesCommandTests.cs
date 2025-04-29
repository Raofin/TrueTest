using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Exams.Commands;
using OPS.Application.Interfaces;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.User;

namespace OPS.Application.Tests.Unit.Features.Exams.Commands;

public class InviteCandidatesCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly InviteCandidatesCommandHandler _sut;
    private readonly InviteCandidatesCommandValidator _validator = new();
    private readonly Examination _existingExam;
    private readonly List<Account> _existingAccounts;

    public InviteCandidatesCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _emailSender = Substitute.For<IEmailSender>();
        _sut = new InviteCandidatesCommandHandler(_unitOfWork, _emailSender);

        _existingExam = new Examination()
        {
            Id = Guid.NewGuid(),
            Title = "Test Exam",
            OpensAt = DateTime.UtcNow.AddDays(1),
            DurationMinutes = 60
        };

        _existingAccounts = new List<Account>
        {
            new() { Id = Guid.NewGuid(), Email = "existing1@example.com" },
            new() { Id = Guid.NewGuid(), Email = "existing2@example.com" }
        };
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldInviteCandidates()
    {
        // Arrange
        var command = new InviteCandidatesCommand(
            _existingExam.Id,
            new List<string> { "new1@example.com", "new2@example.com" }
        );

        _unitOfWork.Exam.GetAsync(command.ExamId, Arg.Any<CancellationToken>())
            .Returns(_existingExam);

        _unitOfWork.Account.GetByEmailsAsync(Arg.Any<List<string>>(), Arg.Any<CancellationToken>())
            .Returns(_existingAccounts);

        _unitOfWork.ExamCandidate.GetEmailsByExamAsync(command.ExamId, Arg.Any<List<string>>(), Arg.Any<CancellationToken>())
            .Returns(new List<string>());

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _unitOfWork.ExamCandidate.Received(1).AddRange(Arg.Is<List<ExamCandidate>>(candidates =>
            candidates.Count == 2 &&
            candidates.All(c => c.ExaminationId == command.ExamId)));

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _emailSender.Received(1).SendExamInvitation(
            Arg.Is<List<string>>(emails => emails.Count == 2),
            _existingExam.Title,
            _existingExam.OpensAt,
            _existingExam.DurationMinutes,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new InviteCandidatesCommand(
            Guid.NewGuid(),
            new List<string> { "test@example.com" }
        );

        _unitOfWork.Exam.GetAsync(command.ExamId, Arg.Any<CancellationToken>())
            .Returns((Examination?)null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);

        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenSomeCandidatesAlreadyInvited_ShouldSkipExistingCandidates()
    {
        // Arrange
        var command = new InviteCandidatesCommand(
            _existingExam.Id,
            new List<string> { "existing@example.com", "new@example.com" }
        );

        _unitOfWork.Exam.GetAsync(command.ExamId, Arg.Any<CancellationToken>())
            .Returns(_existingExam);

        _unitOfWork.ExamCandidate.GetEmailsByExamAsync(command.ExamId, Arg.Any<List<string>>(), Arg.Any<CancellationToken>())
            .Returns(new List<string> { "existing@example.com" });

        _unitOfWork.Account.GetByEmailsAsync(Arg.Any<List<string>>(), Arg.Any<CancellationToken>())
            .Returns(_existingAccounts);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _unitOfWork.ExamCandidate.Received(1).AddRange(Arg.Is<List<ExamCandidate>>(candidates =>
            candidates.Count == 1 &&
            candidates[0].CandidateEmail == "new@example.com"));

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _emailSender.Received(1).SendExamInvitation(
            Arg.Is<List<string>>(emails => emails.Count == 1 && emails[0] == "new@example.com"),
            _existingExam.Title,
            _existingExam.OpensAt,
            _existingExam.DurationMinutes,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAllCandidatesAlreadyInvited_ShouldNotCallAddOrSendEmail()
    {
        var command = new InviteCandidatesCommand(
            _existingExam.Id,
            new List<string> { "already@example.com" }
        );

        _unitOfWork.Exam.GetAsync(command.ExamId, Arg.Any<CancellationToken>())
            .Returns(_existingExam);

        _unitOfWork.ExamCandidate.GetEmailsByExamAsync(command.ExamId, Arg.Any<List<string>>(), Arg.Any<CancellationToken>())
            .Returns(new List<string> { "already@example.com" });

        _unitOfWork.Account.GetByEmailsAsync(Arg.Any<List<string>>(), Arg.Any<CancellationToken>())
            .Returns(new List<Account>());

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _unitOfWork.ExamCandidate.DidNotReceive().AddRange(Arg.Any<List<ExamCandidate>>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
        _emailSender.DidNotReceive().SendExamInvitation(
            Arg.Any<List<string>>(), Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<int>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new InviteCandidatesCommand(
            Guid.NewGuid(),
            new List<string> { "test1@example.com", "test2@example.com" }
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new InviteCandidatesCommand(
            Guid.Empty,
            new List<string> { "test@example.com" }
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.ExamId);
    }

    [Fact]
    public void Validate_WhenEmailsListIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new InviteCandidatesCommand(
            Guid.NewGuid(),
            new List<string>()
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Emails);
    }

    [Fact]
    public void Validate_WhenEmailsListContainsEmptyEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new InviteCandidatesCommand(
            Guid.NewGuid(),
            new List<string> { "test@example.com", "" }
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Emails);
    }

    [Fact]
    public void Validate_WhenEmailsListContainsWhitespaceEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new InviteCandidatesCommand(
            Guid.NewGuid(),
            new List<string> { "test@example.com", "   " }
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Emails);
    }

    [Fact]
    public void Validate_WhenEmailsListContainsNullEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new InviteCandidatesCommand(
            Guid.NewGuid(),
            new List<string> { "test@example.com", null! }
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Emails);
    }

    [Fact]
    public async Task Handle_WhenExistingAccountFound_ShouldSetAccountId()
    {
        // Arrange
        var command = new InviteCandidatesCommand(
            _existingExam.Id,
            new List<string> { "existing1@example.com" }
        );

        _unitOfWork.Exam.GetAsync(command.ExamId, Arg.Any<CancellationToken>())
            .Returns(_existingExam);

        _unitOfWork.ExamCandidate.GetEmailsByExamAsync(command.ExamId, Arg.Any<List<string>>(), Arg.Any<CancellationToken>())
            .Returns(new List<string>());

        _unitOfWork.Account.GetByEmailsAsync(Arg.Any<List<string>>(), Arg.Any<CancellationToken>())
            .Returns(_existingAccounts);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _unitOfWork.ExamCandidate.Received(1).AddRange(Arg.Is<List<ExamCandidate>>(candidates =>
            candidates.Count == 1 &&
            candidates[0].AccountId == _existingAccounts[0].Id &&
            candidates[0].CandidateEmail == "existing1@example.com"));

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _emailSender.Received(1).SendExamInvitation(
            Arg.Is<List<string>>(emails => emails.Count == 1 && emails[0] == "existing1@example.com"),
            _existingExam.Title,
            _existingExam.OpensAt,
            _existingExam.DurationMinutes,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenNoNewCandidates_ShouldNotCallAddOrSendEmail()
    {
        // Arrange
        var command = new InviteCandidatesCommand(
            _existingExam.Id,
            new List<string> { "existing@example.com" }
        );

        _unitOfWork.Exam.GetAsync(command.ExamId, Arg.Any<CancellationToken>())
            .Returns(_existingExam);

        _unitOfWork.ExamCandidate.GetEmailsByExamAsync(command.ExamId, Arg.Any<List<string>>(), Arg.Any<CancellationToken>())
            .Returns(new List<string> { "existing@example.com" });

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _unitOfWork.ExamCandidate.DidNotReceive().AddRange(Arg.Any<List<ExamCandidate>>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
        _emailSender.DidNotReceive().SendExamInvitation(
            Arg.Any<List<string>>(),
            Arg.Any<string>(),
            Arg.Any<DateTime>(),
            Arg.Any<int>(),
            Arg.Any<CancellationToken>());
    }
}