using BCrypt.Net;
using CompanyInventory.Data;
using CompanyInventory.DTOs;
using CompanyInventory.Interfaces;
using CompanyInventory.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CompanyInventory.Services;

public class AuthService(ApplicationDbContext context) : IAuthService
{
    //public async Task<bool> RegisterAsync(RegisterDto registerDto)
    //{
    //    if (await context.Users.AnyAsync(x => x.Email == registerDto.Email))
    //    {
    //        return false;
    //    }

    //    var user = new User
    //    {
    //        FullName = registerDto.FullName,
    //        Email = registerDto.Email,
    //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
    //        Role = "User",
    //        CreatedBy = registerDto.Email
    //    };

    //    context.Users.Add(user);
    //    await context.SaveChangesAsync();

    //    return true;
    //}

    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        
        // Check if the email is already registered
        if (await context.Users.AnyAsync(x => x.Email == registerDto.Email))
        {
            return false;
        }

        // Find employee in EmployeeMaster
        var employee = await context.EmployeeMasters
            .FirstOrDefaultAsync(x =>
                x.EmployeeCode == registerDto.EmployeeCode &&
                x.Email == registerDto.Email &&
                x.IsActive);

        if (employee == null)
        {
            return false;
        }

        // Check if already registered
        if (employee.IsRegistered)
        {
            return false;
        }

        // Create user from EmployeeMaster
        var user = new User
        {
            FullName = employee.FullName,
            Email = employee.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            Role = employee.Role,
            CreatedBy = employee.Email
        };

        context.Users.Add(user);

        // Mark employee as registered
        employee.IsRegistered = true;
        employee.UpdatedOn = DateTime.Now;
        employee.UpdatedBy = employee.Email;

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> LoginAsync(HttpContext httpContext, LoginDto loginDto)
    {
        //var user = await context.Users
        //    .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        var user = await context.Users.FirstOrDefaultAsync(x =>
        x.Email == loginDto.Email &&
        x.IsActive);

        if (user == null)
        {
            return false;
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
            loginDto.Password,
            user.PasswordHash);

        if (!isPasswordValid)
        {
            return false;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return true;
    }
}