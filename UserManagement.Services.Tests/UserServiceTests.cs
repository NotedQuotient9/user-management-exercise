using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }

    [Fact]
    public void FilterByActive_WhenContextReturnsEntities_MustReturnOnlyActiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var activeUser = new User
        {
            Forename = "Johnny",
            Surname = "User",
            Email = "juser@example.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true
        };
        var inactiveUser = new User
        {
            Forename = "Jane",
            Surname = "User",
            Email = "test@email.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = false
        };
        var users = new[] { activeUser, inactiveUser }.AsQueryable();
        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.FilterByActive(true);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().ContainSingle()
        .Which.Should().BeEquivalentTo(activeUser);
    }

    [Fact]
    public void FilterByActive_WhenContextReturnsEntities_MustReturnOnlyInactiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var activeUser = new User
        {
            Forename = "Johnny",
            Surname = "User",
            Email = "juser@example.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true
        };
        var inactiveUser = new User
        {
            Forename = "Jane",
            Surname = "User",
            Email = "test@email.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = false
        };
        var users = new[] { activeUser, inactiveUser }.AsQueryable();
        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.FilterByActive(false);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().ContainSingle()
        .Which.Should().BeEquivalentTo(inactiveUser);
    }

    [Fact]
    public void Create_WhenCreateCalled_ShouldUseUserDetails()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var userToCreate = new User
        {
            Forename = "Johnny",
            Surname = "User",
            Email = "juser@example.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true,
            Id = 1
        };
        _dataContext
            .Setup(s => s.Create<User>(It.IsAny<User>()))
            .Returns(userToCreate);

        // Act: Invokes the method under test with the arranged parameters.
        service.Create(userToCreate);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(s => s.Create(It.Is<User>(u =>
            u.Forename == userToCreate.Forename &&
            u.Surname == userToCreate.Surname &&
            u.Email == userToCreate.Email &&
            u.IsActive == userToCreate.IsActive)), Times.Once);
    }

    [Fact]
    public void GetById_IfUserExists_ShouldReturnUserDetails()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var user = new User
        {
            Forename = "Johnny",
            Surname = "User",
            Email = "juser@example.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true
        };
        _dataContext
            .Setup(s => s.GetById<User>(It.IsAny<long>()))
            .Returns(user);

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetById(1);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(user);

    }

    [Fact]
    public void GetById_IfUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        _dataContext
            .Setup(s => s.GetById<User>(It.IsAny<long>()))
            .Returns(value: null);

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetById(1);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeNull();

    }

    [Fact]
    public void Update_WhenUpdateCalled_ShouldUseUserDetails()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var userToUpdate = new User
        {
            Forename = "Johnny",
            Surname = "User",
            Email = "juser@example.com",
            IsActive = true,
            Id = 1
        };

        // Act: Invokes the method under test with the arranged parameters.
        service.Update(userToUpdate);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(s => s.Update(It.Is<User>(u =>
            u.Forename == userToUpdate.Forename &&
            u.Surname == userToUpdate.Surname &&
            u.Email == userToUpdate.Email &&
            u.IsActive == userToUpdate.IsActive &&
            u.Id == userToUpdate.Id)), Times.Once);
    }

    [Fact]
    public void Delete_WhenDeleteCalled_ShouldUseUserDetails()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var userToDelete = new User
        {
            Forename = "Johnny",
            Surname = "User",
            Email = "juser@example.com",
            IsActive = true,
            Id = 1
        };

        // Act: Invokes the method under test with the arranged parameters.
        service.Delete(userToDelete);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(s => s.Delete(It.Is<User>(u =>
            u.Forename == userToDelete.Forename &&
            u.Surname == userToDelete.Surname &&
            u.Email == userToDelete.Email &&
            u.IsActive == userToDelete.IsActive &&
            u.Id == userToDelete.Id)), Times.Once);
    }

    [Fact]
    public void Create_WhenCreateCalled_ShouldLogAction()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var userToCreate = new User
        {
            Forename = "Johnny",
            Surname = "User",
            Email = "juser@example.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true,
            Id = 1
        };
        var logToCreate = new Log
        {
            Type = LogType.Created,
            Description = "User: 1 created; Forname: Johnny, Surname: User, Email: juser@example.com, IsActive: True, DateOfBirth: 01/01/1990",
            CreatedAt = DateTime.UtcNow,
            UserId = userToCreate.Id
        };
        _dataContext
            .Setup(s => s.Create<User>(It.IsAny<User>()))
            .Returns(userToCreate);

        // Act: Invokes the method under test with the arranged parameters.
        service.Create(userToCreate);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(s => s.Create(It.Is<Log>(l =>
            l.Type == logToCreate.Type &&
            l.Description == logToCreate.Description &&
            l.CreatedAt != default &&
            l.Id == 0 &&
            l.UserId == logToCreate.UserId)), Times.Once);
    }

    [Fact]
    public void Update_WhenUpdateCalled_ShouldLogAction()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var userToUpdate = new User
        {
            Forename = "Johnny",
            Surname = "User",
            Email = "juser@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1990, 1, 1),
            Id = 1
        };
        var logToCreate = new Log
        {
            Type = LogType.Updated,
            Description = "User: 1 updated; Forname: Johnny, Surname: User, Email: juser@example.com, IsActive: True, DateOfBirth: 01/01/1990",
            CreatedAt = DateTime.UtcNow,
            UserId = userToUpdate.Id
        };

        // Act: Invokes the method under test with the arranged parameters.
        service.Update(userToUpdate);

        // Assert: Verifies that the action of the method under test behaves as expected.
        _dataContext.Verify(s => s.Create(It.Is<Log>(l =>
            l.Type == logToCreate.Type &&
            l.Description == logToCreate.Description &&
            l.CreatedAt != default &&
            l.Id == 0 &&
            l.UserId == logToCreate.UserId)), Times.Once);
    }

    [Fact]
    public void Delete_WhenDeleteCalled_ShouldLogAction()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var userToDelete = new User
        {
            Forename = "Johnny",
            Surname = "User",
            Email = "juser@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1990, 1, 1),
            Id = 1
        };
        var logToCreate = new Log
        {
            Type = LogType.Deleted,
            Description = "User: 1 deleted; Forname: Johnny, Surname: User, Email: juser@example.com, IsActive: True, DateOfBirth: 01/01/1990",
            CreatedAt = DateTime.UtcNow,
            UserId = userToDelete.Id
        };

        // Act: Invokes the method under test with the arranged parameters.
        service.Delete(userToDelete);

        // Assert: Verifies that the action o`f the method under test behaves as expected.
        _dataContext.Verify(s => s.Create(It.Is<Log>(l =>
            l.Type == logToCreate.Type &&
            l.Description == logToCreate.Description &&
            l.CreatedAt != default &&
            l.Id == 0 &&
            l.UserId == logToCreate.UserId)), Times.Once);
    }

    private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true, DateTime dateofBirth = default)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive,
                DateOfBirth = dateofBirth
            }
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private UserService CreateService() => new(_dataContext.Object);
}
