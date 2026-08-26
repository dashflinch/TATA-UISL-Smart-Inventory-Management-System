using CompanyInventory.DTOs;
using Microsoft.AspNetCore.Http;

namespace CompanyInventory.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto registerDto);
    Task<bool> LoginAsync(HttpContext httpContext, LoginDto loginDto);
}