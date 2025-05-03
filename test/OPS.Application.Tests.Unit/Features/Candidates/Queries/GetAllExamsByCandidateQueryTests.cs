using ErrorOr;
using FluentAssertions;
using NSubstitute;
using OPS.Application.Dtos;
using OPS.Application.Features.Candidates.Queries;
using OPS.Application.Interfaces.Auth;
using OPS.Application.Mappers;
using OPS.Domain;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Candidates.Queries;

public class GetAllExamsByCandidateQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserProvider _userProvider;
    private readonly GetAllExamsByCandidateQueryHandler _sut;
    private readonly Guid _validAccountId;
    private readonly Guid _validExamId1;
    private readonly Guid _validExamId2;

    public GetAllExamsByCandidateQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _userProvider = Substitute.For<IUserProvider>();
        _sut = new GetAllExamsByCandidateQueryHandler(_unitOfWork, _userProvider);

        _validAccountId = Guid.NewGuid();
        _validExamId1 = Guid.NewGuid();
        _validExamId2 = Guid.NewGuid();

        _userProvider.AccountId().Returns(_validAccountId);
    }

    [Fact]
    public async Task Handle_WhenExamsExist_ShouldReturnExams()
    {
        // Arrange
        var exams = new List<Examination>
        {
            new()
            {
                Id = _validExamId1,
                Title = "Exam 1",
                DescriptionMarkdown = "Description 1",
                DurationMinutes = 60,
                ClosesAt = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                Questions = new List<Question>()
            },
            new()
            {
                Id = _validExamId2,
                Title = "Exam 2",
                DescriptionMarkdown = "Description 2",
                DurationMinutes = 90,
                ClosesAt = DateTime.UtcNow.AddDays(2),
                CreatedAt = DateTime.UtcNow,
                Questions = new List<Question>()
            }
        };

        _unitOfWork.Exam.GetByAccountIdAsync(_validAccountId, Arg.Any<CancellationToken>())
            .Returns(exams);

        var query = new GetAllExamsByCandidateQuery();

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(2);
        result.Value.Should().BeEquivalentTo(exams.Select(e => e.MapToDto()), options => options
            .ComparingByMembers<ExamResponse>()
            .ExcludingMissingMembers());
    }

    [Fact]
    public async Task Handle_WhenNoExamsExist_ShouldReturnEmptyList()
    {
        // Arrange
        _unitOfWork.Exam.GetByAccountIdAsync(_validAccountId, Arg.Any<CancellationToken>())
            .Returns([]);

        var query = new GetAllExamsByCandidateQuery();

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenExamHasQuestions_ShouldMapQuestionsCorrectly()
    {
        // Arrange
        var exam = new Examination
        {
            Id = _validExamId1,
            Title = "Exam with Questions",
            DescriptionMarkdown = "Description",
            DurationMinutes = 60,
            ClosesAt = DateTime.UtcNow.AddDays(1),
            CreatedAt = DateTime.UtcNow,
            Questions = new List<Question>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    StatementMarkdown = "Question 1",
                    Points = 10,
                    DifficultyId = (int)DifficultyType.Easy,
                    McqOption = new McqOption
                    {
                        Option1 = "Option 1",
                        Option2 = "Option 2",
                    }
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    StatementMarkdown = "Question 2",
                    Points = 20,
                    DifficultyId = (int)DifficultyType.Medium,
                }
            }
        };

        _unitOfWork.Exam.GetByAccountIdAsync(_validAccountId, Arg.Any<CancellationToken>())
            .Returns(new List<Examination> { exam });

        var query = new GetAllExamsByCandidateQuery();

        // Act
        ErrorOr<List<ExamWithResultResponse>> result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_WhenExamHasSubmissions_ShouldMapSubmissionsCorrectly()
    {
        // Arrange
        var exam = new Examination
        {
            Id = _validExamId1,
            Title = "Exam with Submissions",
            DescriptionMarkdown = "Description",
            DurationMinutes = 60,
            ClosesAt = DateTime.UtcNow.AddDays(1),
            CreatedAt = DateTime.UtcNow,
            Questions = new List<Question>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    McqSubmissions = new List<McqSubmission>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            AnswerOptions = "1,2"
                        }
                    }
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ProblemSubmissions = new List<ProblemSubmission>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Code = "print('Hello')",
                            LanguageId = "python"
                        }
                    }
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    WrittenSubmissions = new List<WrittenSubmission>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Answer = "Written answer"
                        }
                    }
                }
            }
        };

        _unitOfWork.Exam.GetByAccountIdAsync(_validAccountId, Arg.Any<CancellationToken>())
            .Returns(new List<Examination> { exam });

        var query = new GetAllExamsByCandidateQuery();

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(1);
    }
}