using CompanyInventory.DTOs;
using CompanyInventory.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CompanyInventory.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {


        if (!ModelState.IsValid)
        {
            return View(registerDto);
        }


        bool result = await authService.RegisterAsync(registerDto);

        if (!result)
        {
            ModelState.AddModelError("", "Registration failed. Please check your Employee Code, Email, or if you have already registered.");
            return View(registerDto);
        }

        TempData["Success"] = "Registration successful. Please login.";

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return View(loginDto);
        }

        bool success = await authService.LoginAsync(HttpContext, loginDto);

        if (!success)
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View(loginDto);
        }

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
}