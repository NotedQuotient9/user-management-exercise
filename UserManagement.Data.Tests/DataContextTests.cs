using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Data.Tests;

public class DataContextTests
{
    [Fact]
    public async Task GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        var entity = new User
        {
            Forename = "Brand New",
            Surname = "User",
            Email = "brandnewuser@example.com"
        };
        await context.Create(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().Contain(s => s.Email == entity.Email)
            .Which.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public async Task GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entities = await context.GetAll<User>();
        var entity = entities.First();
        context.Delete(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotContain(s => s.Email == entity.Email);
    }

    [Fact]
    public async Task GetAll_WhenFilteredByIsActive_MustOnlyReturnActiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        // Act: Invokes the method under test with the arranged parameters.
        var results = await context.GetAll<User>();
        var result = results.Where(u => u.IsActive == true);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.All(u => u.IsActive).Should().BeTrue();
    }

    [Fact]
    public async void GetAll_WhenFilteredByIsActive_MustOnlyReturnInactiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();


        // Act: Invokes the method under test with the arranged parameters.
        var results = await context.GetAll<User>();
        var result = results.Where(u => u.IsActive == false);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.All(u => u.IsActive).Should().BeFalse();
    }

    [Fact]
    public async Task GetById_WhenUserExists_ShouldReturnUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var expected = new User { Id = 1000, Forename = "New", Surname = "User", Email = "newUser@example.com", IsActive = true };
        await context.Create(expected);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await context.GetById<User>(1000);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetById_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await context.GetById<User>(100);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().BeNull();
    }

    private DataContext CreateContext() => new();
}
