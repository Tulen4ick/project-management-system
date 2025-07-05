using Microsoft.AspNetCore.Identity;
using project_management_system.Models;
using project_management_system.Services.Interfaces;

namespace project_management_system.Services.Implementations;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<User> _passwordHasher = new();
    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);

        return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
    }

}