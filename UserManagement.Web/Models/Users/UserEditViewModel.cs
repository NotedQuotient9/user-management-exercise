namespace UserManagement.Web.Models.Users;

public class UserEditViewModel
{
    public required string Forename { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public required bool IsActive { get; set; }
    public required long Id { get; set; }
}
