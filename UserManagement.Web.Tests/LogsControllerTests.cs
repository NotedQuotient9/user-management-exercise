using System;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logs;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class LogsControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsLogs_ModelMustContainLogs()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var logs = SetupLogs();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<LogListViewModel>()
            .Which.Items.Should().BeEquivalentTo(logs);
    }

    [Fact]
    public void View_WhenServiceReturnsLog_ModelMustContainLog()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var expected = new Log
        {
            Type = LogType.Created,
            Description = "User: 1 created; Forname: Johnny, Surname: User, Email: juser@example.com, IsActive: True, DateOfBirth: 01/01/1990",
            CreatedAt = DateTime.UtcNow,
            UserId = 1,
            Id = 1
        };
        _logService
            .Setup(s => s.GetById(1))
            .Returns(expected);

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.View(1);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<LogListItemViewModel>()
            .Which.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void UserLogs_WhenServiceReturnsLogs_ModelMustContainLogsForUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
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
        };

        _logService
            .Setup(s => s.GetByUserId(1))
                .Returns(logs);

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.UserLogs(1);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<LogListViewModel>()
                .Which.Items.Should().HaveCount(2).And.OnlyContain(log => log.UserId == 1);
    }

    private Log[] SetupLogs()
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
        };

        _logService
            .Setup(s => s.GetAll())
            .Returns(logs);

        return logs;
    }

    private readonly Mock<ILogService> _logService = new();
    private LogsController CreateController() => new(_logService.Object);
}
