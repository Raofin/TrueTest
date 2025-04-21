using ErrorOr;
using FluentAssertions;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.Mcq.Commands;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Questions.Mcq.Commands;

public class CreateMcqCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateMcqQuestionCommandHandler _sut;
    private readonly Examination _exam;
    private readonly Guid _validExamId;
    private readonly Guid _nonExistentExamId;
    private readonly List<CreateMcqQuestionRequest> _validQuestions;

    public CreateMcqCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new CreateMcqQuestionCommandHandler(_unitOfWork);

        _validExamId = Guid.NewGuid();
        _nonExistentExamId = Guid.NewGuid();

        _exam = new Examination
        {
            Id = _validExamId,
            IsPublished = false,
            McqPoints = 0
        };

        _validQuestions =
        [
            new CreateMcqQuestionRequest(
                "What is the capital of France?",
                10,
                DifficultyType.Easy,
                new CreateMcqOptionRequest(
                    "Paris",
                    "London",
                    "Berlin",
                    "Madrid",
                    false,
                    "1"
                )
            ),

            new CreateMcqQuestionRequest(
                "Which of these are programming languages?",
                15,
                DifficultyType.Medium,
                new CreateMcqOptionRequest(
                    "C#",
                    "Java",
                    "Python",
                    "HTML",
                    true,
                    "1,2,3"
                )
            )
        ];

        // Set up default return values
        _unitOfWork.Exam.GetAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Examination)null!);
        _unitOfWork.Exam.GetAsync(_validExamId, Arg.Any<CancellationToken>())
            .Returns(_exam);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);
    }

    [Fact]
    public async Task Handle_WhenExamExistsAndNotPublished_ShouldCreateQuestions()
    {
        // Arrange
        var command = new CreateMcqCommand(_validExamId, _validQuestions);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(2);
        result.Value.Should().AllSatisfy(q =>
        {
            q.Should().BeOfType<McqQuestionResponse>();
            q.StatementMarkdown.Should().BeOneOf(_validQuestions.Select(r => r.StatementMarkdown));
            q.Score.Should().BeOneOf(_validQuestions.Select(r => r.Points));
            q.DifficultyType.Should().BeOneOf(_validQuestions.Select(r => r.DifficultyType));
        });

        _unitOfWork.Question.Received(1)
            .AddRange(Arg.Is<List<Question>>(questions =>
                questions.Count == 2 &&
                questions.All(q => q.ExaminationId == _validExamId) &&
                questions.All(q => q.QuestionTypeId == (int)QuestionType.MCQ) &&
                questions.All(q => q.McqOption != null)));

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _exam.McqPoints.Should().Be(_validQuestions.Sum(q => q.Points));
    }

    [Fact]
    public async Task Handle_WhenExamDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new CreateMcqCommand(_nonExistentExamId, _validQuestions);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        _unitOfWork.Question.DidNotReceive().AddRange(Arg.Any<List<Question>>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamIsPublished_ShouldReturnConflictError()
    {
        // Arrange
        _exam.IsPublished = true;
        var command = new CreateMcqCommand(_validExamId, _validQuestions);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        _unitOfWork.Question.DidNotReceive().AddRange(Arg.Any<List<Question>>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ShouldReturnUnexpectedError()
    {
        // Arrange
        var command = new CreateMcqCommand(_validExamId, _validQuestions);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        _unitOfWork.Question.Received(1).AddRange(Arg.Any<List<Question>>());
    }

    [Fact]
    public void Validate_WhenValidOptions_ShouldReturnSuccess()
    {
        // Arrange
        var request = new CreateMcqOptionRequest(
            Option1: "Option A",
            Option2: "Option B",
            Option3: "Option C",
            Option4: "Option D",
            IsMultiSelect: false,
            AnswerOptions: "1"
        );

        var validator = new CreateMcqOptionRequestValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenValidQuestion_ShouldReturnSuccess()
    {
        // Arrange
        var question = new CreateMcqQuestionRequest(
            StatementMarkdown: "What's the correct answer?",
            Points: 10,
            DifficultyType: DifficultyType.Medium,
            McqOption: new CreateMcqOptionRequest(
                Option1: "A",
                Option2: "B",
                Option3: "C",
                Option4: "D",
                IsMultiSelect: true,
                AnswerOptions: "1,2"
            )
        );

        var validator = new CreateMcqQuestionRequestValidator();

        // Act
        var result = validator.Validate(question);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldReturnSuccess()
    {
        // Arrange
        var command = new CreateMcqCommand(
            ExamId: Guid.NewGuid(),
            McqQuestions:
            [
                new CreateMcqQuestionRequest(
                    StatementMarkdown: "Sample question?",
                    Points: 20,
                    DifficultyType: DifficultyType.Hard,
                    McqOption: new CreateMcqOptionRequest(
                        Option1: "Yes",
                        Option2: "No",
                        Option3: "Maybe",
                        Option4: "Not Sure",
                        IsMultiSelect: false,
                        AnswerOptions: "1"
                    )
                )
            ]
        );

        var validator = new CreateMcqCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}