using project_management_system.Models;
using project_management_system.Services.Interfaces;

namespace project_management_system.Services.Implementations;

public class UserService : IUserService
{
    private readonly IDataStorage _storage;

    private readonly IPasswordService _passwordService;
    public UserService(IDataStorage storage, IPasswordService passwordService)
    {
        _storage = storage;
        _passwordService = passwordService;
    }
    public void RegisterUser(User user)
    {
        if (UserExists(user.Login))
        {
            throw new ArgumentException("User with that login is already exists");
        }
        if (user.Login == null || user.Password == null)
        {
            throw new NullReferenceException("Login or password is null");
        }
        var data = _storage.LoadData();
        user.Password = _passwordService.HashPassword(user, user.Password);
        data.Users.Add(user);
        _storage.SaveData(data);
    }

    public bool UserExists(string login)
    {
        return _storage.LoadData().Users.Any(x => x.Login == login);
    }

    public User? FindUserByLogin(string login)
    {
        return _storage.LoadData().Users.FirstOrDefault(x => x.Login == login);
    }

    public void CreateSuperUser()
    {
        var data = _storage.LoadData();
        if (data.Users.Count == 0)
        {
            var InitialSuperUser = new User
            {
                Login = "Admin",
                Password = "",
                Role = UserRoles.Manager
            };
            InitialSuperUser.Password = _passwordService.HashPassword(InitialSuperUser, "Admin123");
            data.Users.Add(InitialSuperUser);
            _storage.SaveData(data);
        }
    }
}