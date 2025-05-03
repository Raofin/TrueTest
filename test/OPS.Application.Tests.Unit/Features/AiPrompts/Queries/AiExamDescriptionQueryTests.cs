using FluentAssertions;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.AiPrompts.Queries;
using OPS.Application.Interfaces;

namespace OPS.Application.Tests.Unit.Features.AiPrompts.Queries;

public class AiExamDescriptionQueryTests
{
    private readonly IAiService _aiService;
    private readonly AiExamDescriptionQueryHandler _sut;
    private readonly AiExamDescriptionQueryValidator _validator = new();


    public AiExamDescriptionQueryTests()
    {
        _aiService = Substitute.For<IAiService>();
        _sut = new AiExamDescriptionQueryHandler(_aiService);
    }

    [Fact]
    public async Task Handle_WhenAiServiceReturnsResponse_ReturnsSuccess()
    {
        // Arrange
        var query = new AiExamDescriptionQuery("Test Title", "Test Prompt");
        var expectedResponse = new AiExamDescriptionResponse("Test Description");

        _aiService.PromptAsync<AiExamDescriptionResponse>(Arg.Any<PromptRequest>())
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
        var query = new AiExamDescriptionQuery("Test Title", "Test Prompt");

        _aiService.PromptAsync<AiExamDescriptionResponse>(Arg.Any<PromptRequest>())
            .Returns((AiExamDescriptionResponse)null!);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("Test Title", null)]
    [InlineData(null, "Test Prompt")]
    [InlineData("Test Title", "Test Prompt")]
    public async Task Handle_WithValidInput_ShouldNotThrowException(string? title, string? userPrompt)
    {
        // Arrange
        var query = new AiExamDescriptionQuery(title, userPrompt);
        var expectedResponse = new AiExamDescriptionResponse("Test Description");

        _aiService.PromptAsync<AiExamDescriptionResponse>(Arg.Any<PromptRequest>())
            .Returns(expectedResponse);

        // Act
        var act = () => _sut.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("Test Title", null)]
    [InlineData(null, "Test Prompt")]
    [InlineData("Test Title", "Test Prompt")]
    public void Validate_WithValidInput_ShouldNotHaveErrors(string? title, string? userPrompt)
    {
        // Arrange
        var query = new AiExamDescriptionQuery(title, userPrompt);

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}