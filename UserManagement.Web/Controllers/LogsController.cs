using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logs;

namespace UserManagement.WebMS.Controllers;

[Route("logs")]
public class LogsController : Controller
{
    private readonly ILogService _logService;
    public LogsController(ILogService logService) => _logService = logService;

    [HttpGet]
    public ViewResult List()
    {
        var logs = _logService.GetAll();
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
    public ViewResult View(long id)
    {

        var log = _logService.GetById(id);

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
    public PartialViewResult UserLogs(long userId)
    {
        var logs = _logService.GetByUserId(userId);
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
