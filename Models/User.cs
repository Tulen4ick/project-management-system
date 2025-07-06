namespace project_management_system.Models;

using System.ComponentModel.DataAnnotations;
public class User
{
    [Required]
    [RegularExpression("^[a-zA-Zа-яА-Я0-9]+$", ErrorMessage = "the login contains invalid characters")]
    public required string Login { get; set; }
    [Required]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "the password contains invalid characters")]
    public required string Password { get; set; }
    public UserRoles Role { get; set; } = UserRoles.Employee;
}