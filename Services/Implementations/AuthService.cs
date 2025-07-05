using System.Runtime.InteropServices;
using project_management_system.Models;
using project_management_system.Services.Interfaces;

namespace project_management_system.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IDataStorage _storage;
    private readonly IPasswordService _passwordService;
    public AuthService(IDataStorage storage, IPasswordService passwordService)
    {
        _storage = storage;
        _passwordService = passwordService;
    }
    public User? Login(string? login, string? password)
    {
        var data = _storage.LoadData();
        if (login == null || password == null)
        {
            throw new NullReferenceException("Login or Password is null");
        }
        var user = data.Users.FirstOrDefault(x => x.Login == login) ?? throw new KeyNotFoundException("User with that login is not found");
        var isValid = _passwordService.VerifyPassword(user, user.Password, password);
        return isValid ? user : null;
    }
}