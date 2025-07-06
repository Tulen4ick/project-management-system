using project_management_system.Models;

namespace project_management_system.Services.Interfaces;

public interface IUserService
{
    void RegisterUser(User user);
    bool UserExists(string login);
    public List<User> GetAllEmployees();
    User? FindUserByLogin(string login);
    public void CreateSuperUser();
}