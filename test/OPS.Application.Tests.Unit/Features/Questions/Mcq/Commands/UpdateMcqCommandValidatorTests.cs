using FluentValidation.TestHelper;
using OPS.Application.Features.Questions.Mcq.Commands;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Questions.Mcq.Commands;

public class UpdateMcqCommandValidatorTests
{
    private readonly UpdateMcqCommandValidator _validator = new();

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateMcqCommand(
            Guid.NewGuid(),
            "What is the capital of France?",
            10,
            DifficultyType.Easy,
            new UpdateMcqOptionRequest(
                "option 1",
                "option 2",
                "option 3",
                "option 4",
                false,
                "1"
            )
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenQuestionIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateMcqCommand(
            Guid.Empty,
            "Valid question",
            10,
            DifficultyType.Easy,
            new UpdateMcqOptionRequest(
                "option 1",
                "option 2",
                "option 3",
                "option 4",
                false,
                "1"
            )
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.QuestionId);
    }

    [Fact]
    public void Validate_WhenStatementIsTooShort_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateMcqCommand(
            Guid.NewGuid(),
            "Short", // Less than 10 characters
            10,
            DifficultyType.Easy,
            new UpdateMcqOptionRequest(
                "option 1",
                "option 2",
                "option 3",
                "option 4",
                false,
                "1"
            )
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.StatementMarkdown);
    }

    [Fact]
    public void Validate_WhenPointsIsZero_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateMcqCommand(
            Guid.NewGuid(),
            "Valid question",
            0, // Invalid points
            DifficultyType.Easy,
            new UpdateMcqOptionRequest(
                "option 1",
                "option 2",
                "option 3",
                "option 4",
                false,
                "1"
            )
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Points);
    }

    [Fact]
    public void Validate_WhenPointsIsGreaterThan100_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateMcqCommand(
            Guid.NewGuid(),
            "Valid question",
            101, // Invalid points
            DifficultyType.Easy,
            new UpdateMcqOptionRequest(
                "option 1",
                "option 2",
                "option 3",
                "option 4",
                false,
                "1"
            )
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Points);
    }

    [Fact]
    public void Validate_WhenDifficultyTypeIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateMcqCommand(
            Guid.NewGuid(),
            "Valid question",
            10,
            (DifficultyType)999, // Invalid difficulty
            new UpdateMcqOptionRequest(
                "option 1",
                "option 2",
                "option 3",
                "option 4",
                false,
                "1"
            )
        );

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.DifficultyType);
    }

    [Fact]
    public void Validate_WhenAllFieldsAreNull_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new UpdateMcqCommand(
            Guid.NewGuid(),
            null,
            null,
            null,
            null
        );

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }
}