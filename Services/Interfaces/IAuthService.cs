using project_management_system.Models;
namespace project_management_system.Services.Interfaces;

public interface IAuthService
{
    User? Login(string? login, string? password);
}