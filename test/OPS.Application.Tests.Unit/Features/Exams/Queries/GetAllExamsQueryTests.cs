using FluentAssertions;
using NSubstitute;
using OPS.Application.Features.Exams.Queries;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Entities.Exam;

namespace OPS.Application.Tests.Unit.Features.Exams.Queries;

public class GetAllExamsQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetAllExamsQueryHandler _sut;
    private readonly List<Examination> _exams;

    public GetAllExamsQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetAllExamsQueryHandler(_unitOfWork);

        _exams = new List<Examination>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Exam 1",
                DescriptionMarkdown = "Description 1",
                DurationMinutes = 60,
                TotalPoints = 100,
                OpensAt = DateTime.UtcNow.AddDays(1),
                ClosesAt = DateTime.UtcNow.AddDays(2),
                IsPublished = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Exam 2",
                DescriptionMarkdown = "Description 2",
                DurationMinutes = 90,
                TotalPoints = 150,
                OpensAt = DateTime.UtcNow.AddDays(3),
                ClosesAt = DateTime.UtcNow.AddDays(4),
                IsPublished = true
            }
        };
    }

    [Fact]
    public async Task Handle_WhenExamsExist_ShouldReturnListOfExams()
    {
        // Arrange
        _unitOfWork.Exam.GetAsync(Arg.Any<CancellationToken>())
            .Returns(_exams);

        var query = new GetAllExamsQuery();

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(2);
        result.Value.Should().BeEquivalentTo(_exams.Select(e => e.MapToDto()));
        await _unitOfWork.Exam.Received(1).GetAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenNoExamsExist_ShouldReturnEmptyList()
    {
        // Arrange
        _unitOfWork.Exam.GetAsync(Arg.Any<CancellationToken>())
            .Returns(new List<Examination>());

        var query = new GetAllExamsQuery();

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEmpty();
        await _unitOfWork.Exam.Received(1).GetAsync(Arg.Any<CancellationToken>());
    }
}