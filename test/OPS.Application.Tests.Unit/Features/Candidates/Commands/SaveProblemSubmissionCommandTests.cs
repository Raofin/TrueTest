using ErrorOr;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Features.Candidates.Commands;
using OPS.Domain;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Contracts.Core.OneCompiler;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Candidates.Commands;

public class SaveProblemSubmissionCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInfoProvider _userInfoProvider;
    private readonly IOneCompilerApiService _oneCompilerApi;
    private readonly SaveProblemSubmissionsCommandHandler _sut;
    private readonly SaveProblemSubmissionsCommandValidator _validator = new();
    private readonly Guid _validExamId;
    private readonly Guid _validQuestionId;
    private readonly Guid _validAccountId;
    private readonly Guid _validTestCaseId;

    public SaveProblemSubmissionCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _userInfoProvider = Substitute.For<IUserInfoProvider>();
        _oneCompilerApi = Substitute.For<IOneCompilerApiService>();
        _sut = new SaveProblemSubmissionsCommandHandler(_unitOfWork, _userInfoProvider, _oneCompilerApi);

        _validExamId = Guid.NewGuid();
        _validQuestionId = Guid.NewGuid();
        _validAccountId = Guid.NewGuid();
        _validTestCaseId = Guid.NewGuid();

        _userInfoProvider.AccountId().Returns(_validAccountId);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldSaveSubmissionAndReturnTestResults()
    {
        // Arrange
        var command = new SaveProblemSubmissionsCommand(
            _validExamId,
            _validQuestionId,
            "print('Hello')",
            LanguageId.python
        );

        _unitOfWork.ExamCandidate.IsValidCandidate(_validAccountId, _validExamId, Arg.Any<CancellationToken>())
            .Returns(true);

        var testCase = new TestCase
        {
            Id = _validTestCaseId,
            Input = "test input",
            ExpectedOutput = "Hello"
        };

        _unitOfWork.TestCase.GetByQuestionIdAsync(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(new List<TestCase> { testCase });

        _unitOfWork.Question.GetPointsAsync(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(10);

        _oneCompilerApi.CodeRunAsync(
            Arg.Is<LanguageId>(l => l == LanguageId.python),
            Arg.Is<string>(c => c == "print('Hello')"),
            Arg.Is<string>(i => i == "test input")
        ).Returns(new CodeRunResponse("Hello", null, null, null, null, 100));

        _unitOfWork.ProblemSubmission.GetAsync(_validQuestionId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns((ProblemSubmission)null!);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(1);
        result.Value[0].Should().BeEquivalentTo(new TestCodeResponse(
            _validTestCaseId,
            false,
            null,
            null,
            null,
            100
        ));

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _unitOfWork.ProblemSubmission.Received(1).Add(Arg.Is<ProblemSubmission>(s =>
            s.QuestionId == _validQuestionId &&
            s.AccountId == _validAccountId &&
            s.Code == "print('Hello')" &&
            s.LanguageId == LanguageId.python.ToString() &&
            s.Score == 0 &&
            s.Attempts == 1));
    }

    [Fact]
    public async Task Handle_WhenNotValidCandidate_ShouldReturnForbiddenError()
    {
        // Arrange
        var command = new SaveProblemSubmissionsCommand(
            _validExamId,
            _validQuestionId,
            "print('Hello')",
            LanguageId.python
        );

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
        var command = new SaveProblemSubmissionsCommand(
            _validExamId,
            _validQuestionId,
            "print('Hello')",
            LanguageId.python
        );

        _unitOfWork.ExamCandidate.IsValidCandidate(_validAccountId, _validExamId, Arg.Any<CancellationToken>())
            .Returns(true);

        var testCase = new TestCase
        {
            Id = _validTestCaseId,
            Input = "test input",
            ExpectedOutput = "Hello"
        };

        _unitOfWork.TestCase.GetByQuestionIdAsync(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(new List<TestCase> { testCase });

        _unitOfWork.Question.GetPointsAsync(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(10);

        _oneCompilerApi.CodeRunAsync(
            Arg.Is<LanguageId>(l => l == LanguageId.python),
            Arg.Is<string>(c => c == "print('Hello')"),
            Arg.Is<string>(i => i == "test input")
        ).Returns(new CodeRunResponse("Hello", null, null, null, null, 100));

        var existingSubmission = new ProblemSubmission
        {
            QuestionId = _validQuestionId,
            AccountId = _validAccountId,
            Code = "old code",
            LanguageId = LanguageId.java.ToString(),
            Score = 0,
            Attempts = 1
        };

        _unitOfWork.ProblemSubmission.GetAsync(_validQuestionId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns(existingSubmission);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(1);
        result.Value[0].Should().BeEquivalentTo(new TestCodeResponse(
            _validTestCaseId,
            false,
            null,
            null,
            null,
            100
        ));

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        existingSubmission.Code.Should().Be("print('Hello')");
        existingSubmission.LanguageId.Should().Be(LanguageId.python.ToString());
        existingSubmission.Score.Should().Be(0);
        existingSubmission.Attempts.Should().Be(2);
    }

    [Fact]
    public async Task Handle_WhenTestFails_ShouldSetScoreToZero()
    {
        // Arrange
        var command = new SaveProblemSubmissionsCommand(
            _validExamId,
            _validQuestionId,
            "print('Wrong')",
            LanguageId.python
        );

        _unitOfWork.ExamCandidate.IsValidCandidate(_validAccountId, _validExamId, Arg.Any<CancellationToken>())
            .Returns(true);

        var testCase = new TestCase
        {
            Id = _validTestCaseId,
            Input = "test input",
            ExpectedOutput = "Hello"
        };

        _unitOfWork.TestCase.GetByQuestionIdAsync(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(new List<TestCase> { testCase });

        _unitOfWork.Question.GetPointsAsync(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(10);

        _oneCompilerApi.CodeRunAsync(
            Arg.Is<LanguageId>(l => l == LanguageId.python),
            Arg.Is<string>(c => c == "print('Wrong')"),
            Arg.Is<string>(i => i == "test input")
        ).Returns(new CodeRunResponse("Wrong", null, null, null, null, 100));

        _unitOfWork.ProblemSubmission.GetAsync(_validQuestionId, _validAccountId, Arg.Any<CancellationToken>())
            .Returns((ProblemSubmission)null!);

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(1);
        result.Value[0].Should().BeEquivalentTo(new TestCodeResponse(
            _validTestCaseId,
            false,
            null,
            null,
            null,
            100
        ));

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _unitOfWork.ProblemSubmission.Received(1).Add(Arg.Is<ProblemSubmission>(s =>
            s.QuestionId == _validQuestionId &&
            s.AccountId == _validAccountId &&
            s.Code == "print('Wrong')" &&
            s.LanguageId == LanguageId.python.ToString() &&
            s.Score == 0 &&
            s.Attempts == 1));
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new SaveProblemSubmissionsCommand(
            _validExamId,
            _validQuestionId,
            "print('Hello')",
            LanguageId.python
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new SaveProblemSubmissionsCommand(
            Guid.Empty,
            _validQuestionId,
            "print('Hello')",
            LanguageId.python
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("ExamId");
    }

    [Fact]
    public void Validate_WhenQuestionIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new SaveProblemSubmissionsCommand(
            _validExamId,
            Guid.Empty,
            "print('Hello')",
            LanguageId.python
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("QuestionId");
    }

    [Fact]
    public void Validate_WhenCodeIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new SaveProblemSubmissionsCommand(
            _validExamId,
            _validQuestionId,
            "",
            LanguageId.python
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("Code");
    }

    [Fact]
    public void Validate_WhenLanguageIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new SaveProblemSubmissionsCommand(
            _validExamId,
            _validQuestionId,
            "print('Hello')",
            (LanguageId)999 // Invalid language ID
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("Language");
    }
}