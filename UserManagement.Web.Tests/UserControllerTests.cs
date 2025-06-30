using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(null);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task List_WhenServiceReturnsUsersWithIsActiveTrue_ModelMustContainOnlyActiveUsers()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(isActive: true);

        _userService
            .Setup(s => s.FilterByActive(true))
            .Returns(Task.FromResult(users.AsEnumerable()));

        // Act
        var result = await controller.List(true);

        // Assert
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task List_WhenServiceReturnsUsersWithIsActiveFalse_ModelMustContainOnlyInactiveUsers()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(isActive: false);

        _userService
            .Setup(s => s.FilterByActive(false))
            .Returns(Task.FromResult(users.AsEnumerable()));

        // Act
        var result = await controller.List(false);

        // Assert
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task Create_WhenModelStateIsValid_CallsServiceToCreateUser()
    {
        // Arrange
        var controller = CreateController();
        var model = new UserCreateViewModel
        {
            Forename = "John",
            Surname = "Doe",
            Email = "test@email.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true
        };

        // Act
        await controller.Create(model);

        // Assert
        _userService.Verify(s => s.Create(It.Is<User>(u =>
            u.Forename == model.Forename &&
            u.Surname == model.Surname &&
            u.Email == model.Email &&
            u.IsActive == model.IsActive)), Times.Once);
    }

    [Fact]
    public async Task Create_WhenModelStateIsInvalid_DoesNotCallServiceToCreateUser()
    {
        // Arrange
        var controller = CreateController();
        var model = new UserCreateViewModel
        {
            Forename = "John",
            Surname = "Doe",
            Email = "test@email.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true
        };
        controller.ModelState.AddModelError("Forename", "Required");

        // Act
        await controller.Create(model);

        // Assert
        _userService.Verify(s => s.Create(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Create_WhenEmailIsTaken_DoesNotCallServiceToCreateUser()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(email: "test@email.com");

        var model = new UserCreateViewModel
        {
            Forename = "John",
            Surname = "Doe",
            Email = "test@email.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true
        };

        // Act
        await controller.Create(model);

        // Assert
        _userService.Verify(s => s.Create(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task View_WhenServiceReturnsUser_ModelMustContainUser()
    {
        // Arrange
        var controller = CreateController();
        var expected = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Doe",
            Email = "test@email.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true
        };

        _userService
            .Setup(s => s.GetById(1))
            .Returns(Task.FromResult<User?>(expected));

        // Act
        var result = await controller.View(1);

        // Assert
        result.Model
            .Should().BeOfType<UserListItemViewModel>()
            .Which.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task View_WhenServiceReturnsNull_ShouldReturnErrorView()
    {
        // Arrange
        var controller = CreateController();

        _userService
            .Setup(s => s.GetById(1))
            .Returns(Task.FromResult<User?>(null));

        // Act
        var result = await controller.View(1);

        // Assert
        result
            .Should().BeOfType<ViewResult>()
            .Which.ViewName.Should().Be("Error");
    }

    [Fact]
    public async Task Edit_WhenModelStateIsValid_CallsServiceToUpdateUser()
    {
        // Arrange
        var controller = CreateController();
        SetupUsers();
        var model = new UserEditViewModel
        {
            Forename = "John",
            Surname = "Doe",
            Email = "test@email.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true,
            Id = 1
        };
        var user = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Doe",
            Email = "test@email.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true
        };
        _userService
            .Setup(s => s.GetById(1))
            .Returns(Task.FromResult<User?>(user));


        // Act
        await controller.Edit(1, model);

        // Assert
        _userService.Verify(s => s.Update(It.Is<User>(u =>
            u.Forename == model.Forename &&
            u.Surname == model.Surname &&
            u.Email == model.Email &&
            u.IsActive == model.IsActive &&
            u.Id == model.Id)), Times.Once);
    }

    [Fact]
    public async Task Edit_WhenModelStateIsInvalid_DoesNotCallServiceToUpdateUser()
    {
        // Arrange
        var controller = CreateController();
        var model = new UserEditViewModel
        {
            Forename = "John",
            Surname = "Doe",
            Email = "test@email.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true,
            Id = 1
        };
        controller.ModelState.AddModelError("Forename", "Required");

        // Act
        await controller.Edit(1, model);

        // Assert
        _userService.Verify(s => s.Create(It.IsAny<User>()), Times.Never);

    }

    [Fact]
    public async Task Edit_WhenServiceReturnsNull_ShouldReturnErrorView()
    {

        // Arrange
        var controller = CreateController();
        _userService
            .Setup(s => s.GetById(1))
            .Returns(Task.FromResult<User?>(null));
        var model = new UserEditViewModel
        {
            Forename = "John",
            Surname = "Doe",
            Email = "test@email.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true,
            Id = 1
        };

        // Act
        await controller.Edit(1, model);

        // Act
        var result = await controller.View(1);

        // Assert
        result
            .Should().BeOfType<ViewResult>()
            .Which.ViewName.Should().Be("Error");
    }

    [Fact]
    public async Task Delete_WhenUserExists_CallsServiceToDeleteUser()
    {
        // Arrange
        var controller = CreateController();
        SetupUsers();
        var user = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Doe",
            Email = "test@email.com",
            IsActive = true
        };
        _userService
            .Setup(s => s.GetById(1))
            .Returns(Task.FromResult<User?>(user));

        // Act
        await controller.Delete(1);

        // Assert
        _userService.Verify(s => s.Delete(It.Is<User>(u =>
            u.Forename == user.Forename &&
            u.Surname == user.Surname &&
            u.Email == user.Email &&
            u.IsActive == user.IsActive &&
            u.Id == user.Id)), Times.Once);
    }

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true, DateTime dateOfBirth = default)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive,
                DateOfBirth = dateOfBirth
            }
        };

        _userService
            .Setup(s => s.GetAll())
            .Returns(Task.FromResult(users.AsEnumerable()));

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);
}
