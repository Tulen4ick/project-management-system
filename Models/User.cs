namespace project_management_system.Models;

using System.ComponentModel.DataAnnotations;
public class User
{
    [Required]
    public required string Login { get; set; }
    [Required]
    public required string Password { get; set; }
    public UserRoles Role { get; set; } = UserRoles.Employee;
}