using FluentAssertions;
using NSubstitute;
using OPS.Application.Features.Accounts.Queries;
using OPS.Domain;
using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Tests.Unit.Features.Accounts.Queries;

public class GetAllAccountsQueryTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetAllAccountsQueryHandler _sut;

    public GetAllAccountsQueryTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new GetAllAccountsQueryHandler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_WhenNoFilters_ShouldReturnAllAccounts()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new() { Id = Guid.NewGuid(), Username = "user1", Email = "user1@example.com" },
            new() { Id = Guid.NewGuid(), Username = "user2", Email = "user2@example.com" }
        };

        var paginatedResult = new PaginatedList<Account>(accounts, 2, 1, 10);
        _unitOfWork.Account.GetAllWithDetails(1, 10, null, null, Arg.Any<CancellationToken>())
            .Returns(paginatedResult);

        var query = new GetAllAccountsQuery(1, 10, null, null);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Accounts.Should().HaveCount(2);
        result.Value.Page.TotalCount.Should().Be(2);
        result.Value.Page.Index.Should().Be(1);
        result.Value.Page.Size.Should().Be(10);
    }

    [Fact]
    public async Task Handle_WhenSearchTermProvided_ShouldReturnFilteredAccounts()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new() { Id = Guid.NewGuid(), Username = "testuser", Email = "test@example.com" }
        };

        var paginatedResult = new PaginatedList<Account>(accounts, 1, 1, 10);
        _unitOfWork.Account.GetAllWithDetails(1, 10, "test", null, Arg.Any<CancellationToken>())
            .Returns(paginatedResult);

        var query = new GetAllAccountsQuery(1, 10, "test", null);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Accounts.Should().HaveCount(1);
        result.Value.Accounts[0].Username.Should().Be("testuser");
    }

    [Fact]
    public async Task Handle_WhenRoleFilterProvided_ShouldReturnFilteredAccounts()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Email = "admin@example.com",
                AccountRoles = new List<AccountRole>
                {
                    new() { RoleId = (int)RoleType.Admin }
                }
            }
        };

        var paginatedResult = new PaginatedList<Account>(accounts, 1, 1, 10);
        _unitOfWork.Account.GetAllWithDetails(1, 10, null, RoleType.Admin, Arg.Any<CancellationToken>())
            .Returns(paginatedResult);

        var query = new GetAllAccountsQuery(1, 10, null, RoleType.Admin);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Accounts.Should().HaveCount(1);
        result.Value.Accounts[0].Username.Should().Be("admin");
    }

    [Fact]
    public async Task Handle_WhenNoAccountsFound_ShouldReturnEmptyList()
    {
        // Arrange
        var paginatedResult = new PaginatedList<Account>(new List<Account>(), 0, 1, 10);
        _unitOfWork.Account.GetAllWithDetails(1, 10, "nonexistent", null, Arg.Any<CancellationToken>())
            .Returns(paginatedResult);

        var query = new GetAllAccountsQuery(1, 10, "nonexistent", null);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Accounts.Should().BeEmpty();
        result.Value.Page.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Handle_WhenPaginationParametersProvided_ShouldReturnCorrectPage()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new() { Id = Guid.NewGuid(), Username = "user1", Email = "user1@example.com" },
            new() { Id = Guid.NewGuid(), Username = "user2", Email = "user2@example.com" }
        };

        var paginatedResult = new PaginatedList<Account>(accounts, 20, 2, 2);
        _unitOfWork.Account.GetAllWithDetails(2, 2, null, null, Arg.Any<CancellationToken>())
            .Returns(paginatedResult);

        var query = new GetAllAccountsQuery(2, 2, null, null);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Accounts.Should().HaveCount(2);
        result.Value.Page.Index.Should().Be(2);
        result.Value.Page.Size.Should().Be(2);
        result.Value.Page.TotalCount.Should().Be(20);
    }

    [Fact]
    public async Task Handle_WhenCombinedFilters_ShouldReturnCorrectlyFilteredAccounts()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Username = "adminuser",
                Email = "admin@example.com",
                AccountRoles = new List<AccountRole>
                {
                    new() { RoleId = (int)RoleType.Admin }
                }
            }
        };

        var paginatedResult = new PaginatedList<Account>(accounts, 1, 1, 10);
        _unitOfWork.Account.GetAllWithDetails(1, 10, "admin", RoleType.Admin, Arg.Any<CancellationToken>())
            .Returns(paginatedResult);

        var query = new GetAllAccountsQuery(1, 10, "admin", RoleType.Admin);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Accounts.Should().HaveCount(1);
        result.Value.Accounts[0].Username.Should().Be("adminuser");
    }
}