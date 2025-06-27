using System;
using UserManagement.Models;

namespace UserManagement.Web.Models.Logs;

public class LogListViewModel
{
    public List<LogListItemViewModel> Items { get; set; } = new();
}

public class LogListItemViewModel
{

    public long Id { get; set; }
    public long UserId { get; set; }
    public LogType Type { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

}
