using FluentAssertions;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.AiPrompts.Queries;
using OPS.Application.Interfaces;

namespace OPS.Application.Tests.Unit.Features.AiPrompts.Queries;

public class AiPromptQueryTests
{
    private readonly IAiService _aiService;
    private readonly AiPromptCommandHandler _sut;
    private readonly AiPromptCommandValidator _validator = new();

    public AiPromptQueryTests()
    {
        _aiService = Substitute.For<IAiService>();
        _sut = new AiPromptCommandHandler(_aiService);
    }

    [Fact]
    public async Task Handle_WhenAiServiceReturnsResponse_ReturnsSuccess()
    {
        // Arrange
        var command = new AiPromptCommand("Test Instruction", ["Test Content"]);
        var expectedResponse = "Test Response";

        _aiService.PromptAsync<string>(Arg.Any<PromptRequest>())
            .Returns(expectedResponse);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task Handle_WhenAiServiceReturnsNull_ReturnsError()
    {
        // Arrange
        var command = new AiPromptCommand("Test Instruction", ["Test Content"]);

        _aiService.PromptAsync<string>(Arg.Any<PromptRequest>())
            .Returns((string)null!);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
    }

    [Theory]
    [InlineData("Test Instruction", new[] { "Test Content" })]
    [InlineData("Test Instruction", new[] { "Content1", "Content2" })]
    public async Task Handle_WithValidInput_ShouldNotThrowException(string instruction, string[] contents)
    {
        // Arrange
        var command = new AiPromptCommand(instruction, contents.ToList());
        var expectedResponse = "Test Response";

        _aiService.PromptAsync<string>(Arg.Any<PromptRequest>())
            .Returns(expectedResponse);

        // Act
        var act = () => _sut.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Theory]
    [InlineData("Test Instruction", new[] { "Test Content" })]
    [InlineData("Test Instruction", new[] { "Content1", "Content2" })]
    public void Validate_WithValidInput_ShouldNotHaveErrors(string instruction, string[] contents)
    {
        // Arrange
        var command = new AiPromptCommand(instruction, contents.ToList());

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}