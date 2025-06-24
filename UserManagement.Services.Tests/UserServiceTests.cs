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
            IsActive = true
        };
        var inactiveUser = new User
        {
            Forename = "Jane",
            Surname = "User",
            Email = "test@email.com",
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
            IsActive = true
        };
        var inactiveUser = new User
        {
            Forename = "Jane",
            Surname = "User",
            Email = "test@email.com",
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

    private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive
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
