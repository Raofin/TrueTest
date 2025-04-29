using FluentAssertions;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.AiPrompts.Queries;
using OPS.Application.Interfaces;

namespace OPS.Application.Tests.Unit.Features.AiPrompts.Queries;

public class AiGenerateWrittenQuesQueryTests
{
    private readonly IAiService _aiService;
    private readonly AiGenerateWrittenQuesQueryHandler _sut;
    private readonly AiGenerateWrittenQuesQueryValidator _validator = new();

    public AiGenerateWrittenQuesQueryTests()
    {
        _aiService = Substitute.For<IAiService>();
        _sut = new AiGenerateWrittenQuesQueryHandler(_aiService);
    }

    [Fact]
    public async Task Handle_WhenAiServiceReturnsResponse_ReturnsSuccess()
    {
        // Arrange
        var query = new AiGenerateWrittenQuesQuery("Test Prompt");
        var expectedResponse = new AiWrittenQues("Test Question Statement");

        _aiService.PromptAsync<AiWrittenQues>(Arg.Any<PromptRequest>())
            .Returns(expectedResponse);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Handle_WhenAiServiceReturnsNull_ReturnsError()
    {
        // Arrange
        var query = new AiGenerateWrittenQuesQuery("Test Prompt");

        _aiService.PromptAsync<AiWrittenQues>(Arg.Any<PromptRequest>())
            .Returns((AiWrittenQues)null!);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("Test Prompt")]
    public async Task Handle_WithValidInput_ShouldNotThrowException(string? userPrompt)
    {
        // Arrange
        var query = new AiGenerateWrittenQuesQuery(userPrompt);
        var expectedResponse = new AiWrittenQues("Test Question Statement");

        _aiService.PromptAsync<AiWrittenQues>(Arg.Any<PromptRequest>())
            .Returns(expectedResponse);

        // Act
        var act = () => _sut.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("Test Prompt")]
    public void Validate_WithValidInput_ShouldNotHaveErrors(string? userPrompt)
    {
        // Arrange
        var query = new AiGenerateWrittenQuesQuery(userPrompt);

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithInvalidInput_ShouldHaveErrors()
    {
        // Arrange
        var query = new AiGenerateWrittenQuesQuery("a".PadRight(2001));

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }
}