using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class LogServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var logs = SetupLogs();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(logs);
    }

    [Fact]
    public void GetByUserId_WhenContextReturnsEntities_MustByFilteredByUserId()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        SetupLogs();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetByUserId(1);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().HaveCount(2).And.OnlyContain(log => log.UserId == 1);
    }

    [Fact]
    public void GetById_WhenContextReturnsEntities_MustReturnOnlyIndividualLog()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        SetupLogs();

        var log = new Log
        {
            Type = LogType.Updated,
            Description = "User: 2 created; Forname: Johnny, Surname: User, Email: juser@example.com, IsActive: True, DateOfBirth: 01/01/1990",
            CreatedAt = DateTime.UtcNow,
            UserId = 2,
            Id = 3
        };
        _dataContext
            .Setup(s => s.GetById<Log>(3))
            .Returns(log);

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetById(3);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(log);
    }

    private IQueryable<Log> SetupLogs()
    {
        var logs = new[]
        {
            new Log
            {
                Type = LogType.Created,
                Description = "User: 1 created; Forname: Johnny, Surname: User, Email: juser@example.com, IsActive: True, DateOfBirth: 01/01/1990",
                CreatedAt = DateTime.UtcNow,
                UserId = 1,
                Id = 1
            },
            new Log
            {
                Type = LogType.Updated,
                Description = "User: 1 updated; Forname: Johnny, Surname: User, Email: juser@example.com, IsActive: True, DateOfBirth: 01/01/1990",
                CreatedAt = DateTime.UtcNow,
                UserId = 1,
                Id = 2
            },
            new Log
            {
                Type = LogType.Updated,
                Description = "User: 2 created; Forname: Johnny, Surname: User, Email: juser@example.com, IsActive: True, DateOfBirth: 01/01/1990",
                CreatedAt = DateTime.UtcNow,
                UserId = 2,
                Id = 3
            }
    }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<Log>())
                .Returns(logs);

        return logs;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private LogService CreateService() => new(_dataContext.Object);
}
