using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Candidates.Commands;
using OPS.Application.Interfaces;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Candidates.Commands;

public class CodeRunCommandTests
{
    private readonly IOneCompilerService _oneCompiler;
    private readonly CodeRunCommandHandler _sut;
    private readonly CodeRunCommandValidator _validator = new();

    public CodeRunCommandTests()
    {
        _oneCompiler = Substitute.For<IOneCompilerService>();
        _sut = new CodeRunCommandHandler(_oneCompiler);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldReturnCodeRunResponse()
    {
        // Arrange
        var code = "print('Hello, World!')";
        var input = "test input";
        var languageId = LanguageId.python;
        var expectedResponse = new CodeRunResponse("Hello, World!", null, null, null, null, null);

        _oneCompiler.CodeRunAsync(languageId, code, input)
            .Returns(expectedResponse);

        var command = new CodeRunCommand(code, input, languageId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        await _oneCompiler.Received(1).CodeRunAsync(languageId, code, input);
    }

    [Fact]
    public async Task Handle_WhenNoInput_ShouldCallApiWithNullInput()
    {
        // Arrange
        var code = "print('Hello, World!')";
        var languageId = LanguageId.python;
        var expectedResponse = new CodeRunResponse("Hello, World!", null, null, null, null, null);

        _oneCompiler.CodeRunAsync(languageId, code, null)
            .Returns(expectedResponse);

        var command = new CodeRunCommand(code, null, languageId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        await _oneCompiler.Received(1).CodeRunAsync(languageId, code, null);
    }

    [Fact]
    public void Validate_WhenValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CodeRunCommand("print('Hello')", "input", LanguageId.python);

        // Act & Assert
        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenCodeIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CodeRunCommand("", "input", LanguageId.python);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Validate_WhenLanguageIdIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CodeRunCommand("print('Hello')", "input", (LanguageId)999);

        // Act & Assert
        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.LanguageId);
    }
}