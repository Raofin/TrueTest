using ErrorOr;
using FluentAssertions;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Questions.Mcq.Commands;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Questions.Mcq.Commands;

public class UpdateMcqCommandTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateMcqQuestionCommandHandler _sut;
    private readonly Question _question;
    private readonly Guid _validQuestionId;
    private readonly Guid _nonExistentQuestionId;

    public UpdateMcqCommandTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new UpdateMcqQuestionCommandHandler(_unitOfWork);

        _validQuestionId = Guid.NewGuid();
        _nonExistentQuestionId = Guid.NewGuid();

        _question = new Question
        {
            Id = _validQuestionId,
            StatementMarkdown = "Original question",
            Points = 10,
            DifficultyId = (int)DifficultyType.Easy,
            McqOption = new McqOption
            {
                Option1 = "Option 1",
                Option2 = "Option 2",
                Option3 = "Option 3",
                Option4 = "Option 4",
                IsMultiSelect = false,
                AnswerOptions = "1"
            },
            Examination = new Examination
            {
                IsPublished = false,
                McqPoints = 10
            }
        };

        // Set up default return values
        _unitOfWork.Question.GetWithMcqOption(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Question)null!);
        _unitOfWork.Question.GetWithMcqOption(_validQuestionId, Arg.Any<CancellationToken>())
            .Returns(_question);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);
    }

    [Fact]
    public async Task Handle_WhenQuestionExistsAndExamNotPublished_ShouldUpdateQuestion()
    {
        // Arrange
        var command = new UpdateMcqCommand(
            _validQuestionId,
            "Updated question",
            15,
            DifficultyType.Medium,
            new UpdateMcqOptionRequest(
                "Updated Option 1",
                "Updated Option 2",
                "Updated Option 3",
                "Updated Option 4",
                true,
                "1,2"
            )
        );

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<McqQuestionResponse>();
        result.Value.StatementMarkdown.Should().Be("Updated question");
        result.Value.Score.Should().Be(15);
        result.Value.DifficultyType.Should().Be(DifficultyType.Medium);

        _question.StatementMarkdown.Should().Be("Updated question");
        _question.Points.Should().Be(15);
        _question.DifficultyId.Should().Be((int)DifficultyType.Medium);
        _question.McqOption!.Option1.Should().Be("Updated Option 1");
        _question.McqOption.Option2.Should().Be("Updated Option 2");
        _question.McqOption.Option3.Should().Be("Updated Option 3");
        _question.McqOption.Option4.Should().Be("Updated Option 4");
        _question.McqOption.IsMultiSelect.Should().BeTrue();
        _question.McqOption.AnswerOptions.Should().Be("1,2");
        _question.Examination.McqPoints.Should().Be(15);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenQuestionDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new UpdateMcqCommand(
            _nonExistentQuestionId,
            "Updated question",
            15,
            DifficultyType.Medium,
            new UpdateMcqOptionRequest(
                "Updated Option 1",
                "Updated Option 2",
                "Updated Option 3",
                "Updated Option 4",
                true,
                "1,2"
            )
        );

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenExamIsPublished_ShouldReturnConflictError()
    {
        // Arrange
        _question.Examination.IsPublished = true;
        var command = new UpdateMcqCommand(
            _validQuestionId,
            "Updated question",
            15,
            DifficultyType.Medium,
            new UpdateMcqOptionRequest(
                "Updated Option 1",
                "Updated Option 2",
                "Updated Option 3",
                "Updated Option 4",
                true,
                "1,2"
            )
        );

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenPartialUpdate_ShouldUpdateOnlySpecifiedFields()
    {
        // Arrange
        var command = new UpdateMcqCommand(
            _validQuestionId,
            null,
            15,
            null,
            new UpdateMcqOptionRequest(
                null,
                "Updated Option 2",
                null,
                null,
                null,
                null
            )
        );

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<McqQuestionResponse>();

        _question.StatementMarkdown.Should().Be("Original question");
        _question.Points.Should().Be(15);
        _question.DifficultyId.Should().Be((int)DifficultyType.Easy);
        _question.McqOption!.Option1.Should().Be("Option 1");
        _question.McqOption.Option2.Should().Be("Updated Option 2");
        _question.McqOption.Option3.Should().Be("Option 3");
        _question.McqOption.Option4.Should().Be("Option 4");
        _question.McqOption.IsMultiSelect.Should().BeFalse();
        _question.McqOption.AnswerOptions.Should().Be("1");
        _question.Examination.McqPoints.Should().Be(15);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }
}