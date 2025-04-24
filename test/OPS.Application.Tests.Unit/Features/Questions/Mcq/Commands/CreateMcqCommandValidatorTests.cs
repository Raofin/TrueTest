using FluentValidation.TestHelper;
using OPS.Application.Features.Questions.Mcq.Commands;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Questions.Mcq.Commands;

public class CreateMcqCommandValidatorTests
{
    private readonly CreateMcqCommandValidator _validator = new();

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateMcqCommand(
            Guid.NewGuid(),
            [
                new CreateMcqQuestionRequest(
                    "What is deadlock?",
                    10,
                    DifficultyType.Easy,
                    new CreateMcqOptionRequest(
                        "option 1",
                        "option 2",
                        "option 3",
                        "option 4",
                        false,
                        "1"
                    )
                )
            ]
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExamIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateMcqCommand(
            Guid.Empty,
            [
                new CreateMcqQuestionRequest(
                    "Valid question",
                    10,
                    DifficultyType.Easy,
                    new CreateMcqOptionRequest(
                        "option 1",
                        "option 2",
                        "option 3",
                        "option 4",
                        false,
                        "1"
                    )
                )
            ]
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.ExamId);
    }

    [Fact]
    public void Validate_WhenMcqQuestionsIsNull_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateMcqCommand(Guid.NewGuid(), null!);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.McqQuestions);
    }

    [Fact]
    public void Validate_WhenMcqQuestionsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateMcqCommand(Guid.NewGuid(), []);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.McqQuestions);
    }

    [Fact]
    public void Validate_WhenQuestionHasInvalidStatement_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateMcqCommand(
            Guid.NewGuid(),
            [
                new CreateMcqQuestionRequest(
                    "", // Empty statement
                    10,
                    DifficultyType.Easy,
                    new CreateMcqOptionRequest(
                        "option 1",
                        "option 2",
                        "option 3",
                        "option 4",
                        false,
                        "1"
                    )
                )
            ]
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("McqQuestions[0].StatementMarkdown");
    }

    [Fact]
    public void Validate_WhenQuestionHasInvalidPoints_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateMcqCommand(
            Guid.NewGuid(),
            [
                new CreateMcqQuestionRequest(
                    "Valid question",
                    0, // Invalid points
                    DifficultyType.Easy,
                    new CreateMcqOptionRequest(
                        "option 1",
                        "option 2",
                        "option 3",
                        "option 4",
                        false,
                        "1"
                    )
                )
            ]
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("McqQuestions[0].Points");
    }

    [Fact]
    public void Validate_WhenQuestionHasInvalidDifficulty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateMcqCommand(
            Guid.NewGuid(),
            [
                new CreateMcqQuestionRequest(
                    "Valid question",
                    10,
                    (DifficultyType)999, // Invalid difficulty
                    new CreateMcqOptionRequest(
                        "option 1",
                        "option 2",
                        "option 3",
                        "option 4",
                        false,
                        "1"
                    )
                )
            ]
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("McqQuestions[0].DifficultyType");
    }

    [Fact]
    public void Validate_WhenQuestionHasNullMcqOption_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateMcqCommand(
            Guid.NewGuid(),
            [
                new CreateMcqQuestionRequest(
                    "Valid question",
                    10,
                    DifficultyType.Easy,
                    null! // Null MCQ option
                )
            ]
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor("McqQuestions[0].McqOption");
    }
}