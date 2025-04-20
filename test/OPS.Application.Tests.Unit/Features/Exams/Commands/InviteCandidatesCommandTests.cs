using ErrorOr;
using FluentAssertions;
using NSubstitute;
using OPS.Application.Features.Exams.Commands;
using OPS.Domain;
using OPS.Domain.Contracts.Core.EmailSender;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.User;

namespace OPS.Application.Tests.Unit.Features.Exams.Commands;

public class InviteCandidatesCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountEmails _accountEmails;
    private readonly InviteCandidatesCommandHandler _sut;
    private readonly Examination _exam;
    private readonly Guid _validExamId;
    private readonly Guid _nonExistentExamId;
    private readonly List<string> _validEmails;
    private readonly List<string> _existingEmails;
    private readonly List<Account> _mockAccounts;

    public InviteCandidatesCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _accountEmails = Substitute.For<IAccountEmails>();
        _sut = new InviteCandidatesCommandHandler(_unitOfWork, _accountEmails);

        _validExamId = Guid.NewGuid();
        _nonExistentExamId = Guid.NewGuid();
        _validEmails = new List<string> { "test1@example.com", "test2@example.com" };
        _existingEmails = new List<string> { "existing@example.com" };

        _exam = new Examination
        {
            Id = _validExamId,
            Title = "Test Exam",
            OpensAt = DateTime.UtcNow.AddDays(1),
            DurationMinutes = 60
        };

        _mockAccounts = new List<Account>
        {
            new() { Id = Guid.NewGuid(), Email = "test1@example.com" },
            new() { Id = Guid.NewGuid(), Email = "test2@example.com" }
        };

        // Set up default return values
        _unitOfWork.Exam.GetAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Examination)null);
        _unitOfWork.Exam.GetAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(_exam);
        _unitOfWork.Account.GetByEmailsAsync(Arg.Any<List<string>>(), Arg.Any<CancellationToken>())
            .Returns(new List<Account>());
        _unitOfWork.ExamCandidate
            .GetEmailsByExamAsync(Arg.Any<Guid>(), Arg.Any<List<string>>(), Arg.Any<CancellationToken>())
            .Returns(new List<string>());
    }

    [Fact]
    public async Task Handle_WhenExamExists_ShouldInviteNewCandidates()
    {
        // Arrange
        var command = new InviteCandidatesCommand(_validExamId, _validEmails);
        _unitOfWork.Account.GetByEmailsAsync(_validEmails, Arg.Any<CancellationToken>())
            .Returns(_mockAccounts);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _unitOfWork.ExamCandidate.Received(1).AddRange(Arg.Any<List<ExamCandidate>>());

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _accountEmails.Received(1).SendExamInvitation(
            Arg.Is<List<string>>(emails => emails.SequenceEqual(_validEmails)),
            _exam.Title,
            _exam.OpensAt,
            _exam.DurationMinutes,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new InviteCandidatesCommand(_nonExistentExamId, _validEmails);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        _unitOfWork.ExamCandidate.DidNotReceive().AddRange(Arg.Any<List<ExamCandidate>>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
        _accountEmails.DidNotReceive().SendExamInvitation(
            Arg.Any<List<string>>(),
            Arg.Any<string>(),
            Arg.Any<DateTime>(),
            Arg.Any<int>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenSomeCandidatesAlreadyExist_ShouldOnlyInviteNewCandidates()
    {
        // Arrange
        var allEmails = _validEmails.Concat(_existingEmails).ToList();
        var command = new InviteCandidatesCommand(_validExamId, allEmails);
        _unitOfWork.Account.GetByEmailsAsync(allEmails, Arg.Any<CancellationToken>())
            .Returns(_mockAccounts);
        _unitOfWork.ExamCandidate.GetEmailsByExamAsync(_validExamId, allEmails, Arg.Any<CancellationToken>())
            .Returns(_existingEmails);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _unitOfWork.ExamCandidate.Received(1).AddRange(Arg.Any<List<ExamCandidate>>());

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());

        _accountEmails.Received(1).SendExamInvitation(
            Arg.Any<List<string>>(),
            Arg.Any<string>(),
            Arg.Any<DateTime>(),
            Arg.Any<int>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAllCandidatesAlreadyExist_ShouldNotInviteAnyone()
    {
        // Arrange
        var command = new InviteCandidatesCommand(_validExamId, _existingEmails);
        _unitOfWork.Account.GetByEmailsAsync(Arg.Any<List<string>>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new List<Account> { new() { Email = _existingEmails[0] } }));
        _unitOfWork.ExamCandidate
            .GetEmailsByExamAsync(_validExamId, Arg.Any<List<string>>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new List<string> { _existingEmails[0] }));

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _unitOfWork.ExamCandidate.DidNotReceive().AddRange(Arg.Any<List<ExamCandidate>>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
        _accountEmails.DidNotReceive().SendExamInvitation(
            Arg.Any<List<string>>(),
            Arg.Any<string>(),
            Arg.Any<DateTime>(),
            Arg.Any<int>(),
            Arg.Any<CancellationToken>());
    }
}