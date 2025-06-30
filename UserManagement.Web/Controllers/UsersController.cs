using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.Models;
using System.Threading.Tasks;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public async Task<ViewResult> List(bool? isActive)
    {

        var users = isActive.HasValue
            ? await _userService.FilterByActive(isActive.Value)
            : await _userService.GetAll();

        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(UserCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var existingUsers = await _userService.GetAll();
        if (existingUsers.Any(u => u.Email == model.Email))
        {
            ModelState.AddModelError("Email", "Email already exists.");
            return View(model);
        }

        var user = new User
        {
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            IsActive = model.IsActive,
            DateOfBirth = model.DateOfBirth
        };
        _userService.Create(user);
        return RedirectToAction("List");
    }

    [HttpGet("{id:long}")]
    public async Task<ViewResult> View(long id)
    {

        var user = await _userService.GetById(id);

        if (user == null)
        {
            return View("Error");
        }

        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        return View(model);
    }

    [HttpGet("edit/{id:long}")]
    public async Task<IActionResult> Edit(long id)
    {
        var user = await _userService.GetById(id);
        if (user == null)
        {
            return View("Error");
        }

        var model = new UserEditViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };
        return View(model);
    }

    [HttpPost("edit/{id:long}")]
    public async Task<IActionResult> Edit(long id, UserEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userService.GetById(id);
        if (user == null)
        {
            return View("Error");
        }

        user.Forename = model.Forename;
        user.Surname = model.Surname;
        user.Email = model.Email;
        user.IsActive = model.IsActive;
        user.DateOfBirth = model.DateOfBirth;

        _userService.Update(user);

        return RedirectToAction("List");
    }

    [HttpPost("delete/{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var user = await _userService.GetById(id);
        if (user == null)
        {
            return View("Error");
        }

        _userService.Delete(user);
        return RedirectToAction("List");
    }
}
