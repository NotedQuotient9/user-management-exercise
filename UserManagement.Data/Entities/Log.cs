using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

public class Log
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [ForeignKey("UserId")]
    public long UserId { get; set; }
    public LogType Type { get; set; } = default!;
    public string Description { get; set; } = default!;
    public System.DateTime CreatedAt { get; set; }
}
