using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List(null);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void List_WhenSerivceReturnsUsersWithIsActiveTrue_ModelMustContainOnlyActiveUsers()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(isActive: true);

        _userService
            .Setup(s => s.FilterByActive(true))
            .Returns(users);

        // Act
        var result = controller.List(true);

        // Assert
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void List_WhenSerivceReturnsUsersWithIsActiveFalse_ModelMustContainOnlyInactiveUsers()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(isActive: false);

        _userService
            .Setup(s => s.FilterByActive(false))
            .Returns(users);

        // Act
        var result = controller.List(false);

        // Assert
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void Create_WhenModelStateIsValid_CallsServiceToCreateUser()
    {
        // Arrange
        var controller = CreateController();
        var model = new UserCreateViewModel
        {
            Forename = "John",
            Surname = "Doe",
            Email = "test@email.com",
            IsActive = true
        };

        // Act
        controller.Create(model);

        // Assert
        _userService.Verify(s => s.Create(It.Is<User>(u =>
            u.Forename == model.Forename &&
            u.Surname == model.Surname &&
            u.Email == model.Email &&
            u.IsActive == model.IsActive)), Times.Once);
    }

    [Fact]
    public void Create_WhenModelStateIsInvalid_DoesNotCallServiceToCreateUser()
    {
        // Arrange
        var controller = CreateController();
        var model = new UserCreateViewModel
        {
            Forename = "John",
            Surname = "Doe",
            Email = "test@email.com",
            IsActive = true
        };
        controller.ModelState.AddModelError("Forename", "Required");

        // Act
        controller.Create(model);

        // Assert
        _userService.Verify(s => s.Create(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public void Create_WhenEmailIsTaken_DoesNotCallServiceToCreateUser()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(email: "test@email.com");

        var model = new UserCreateViewModel
        {
            Forename = "John",
            Surname = "Doe",
            Email = "test@email.com",
            IsActive = true
        };

        // Act
        controller.Create(model);

        // Assert
        _userService.Verify(s => s.Create(It.IsAny<User>()), Times.Never);
    }

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
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
        };

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);
}
