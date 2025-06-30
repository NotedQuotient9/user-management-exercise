using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logs;

namespace UserManagement.WebMS.Controllers;

[Route("logs")]
public class LogsController : Controller
{
    private readonly ILogService _logService;
    public LogsController(ILogService logService) => _logService = logService;

    [HttpGet]
    public async Task<ViewResult> List()
    {
        var logs = await _logService.GetAll();
        var items = logs.Select(p => new LogListItemViewModel
        {
            Id = p.Id,
            UserId = p.UserId,
            Type = p.Type,
            Description = p.Description,
            CreatedAt = p.CreatedAt
        });

        var model = new LogListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }


    [HttpGet("{id:long}")]
    public async Task<ViewResult> View(long id)
    {

        var log = await _logService.GetById(id);

        if (log == null)
        {
            return View("Error");
        }

        var model = new LogListItemViewModel
        {
            Id = log.Id,
            UserId = log.UserId,
            Type = log.Type,
            Description = log.Description,
            CreatedAt = log.CreatedAt
        };

        return View(model);
    }

    [HttpGet("user/{userId:long}")]
    public async Task<PartialViewResult> UserLogs(long userId)
    {
        var logs = await _logService.GetByUserId(userId);
        var items = logs.Select(p => new LogListItemViewModel
        {
            Id = p.Id,
            UserId = p.UserId,
            Type = p.Type,
            Description = p.Description,
            CreatedAt = p.CreatedAt
        });

        var model = new LogListViewModel
        {
            Items = items.ToList()
        };

        return PartialView("_UserLogs", model);
    }
}
