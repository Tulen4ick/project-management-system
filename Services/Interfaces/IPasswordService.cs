using project_management_system.Models;

namespace project_management_system.Services.Interfaces;

public interface IPasswordService
{
    public string HashPassword(User user, string password);
    public bool VerifyPassword(User user, string hashedPassword, string providedPassword);
}