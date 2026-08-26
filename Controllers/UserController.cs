using CompanyInventory.Data;
using Microsoft.AspNetCore.Authorization;
using CompanyInventory.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyInventory.Controllers;

[Authorize(Roles = "Admin")]
public class UserController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(string? searchString, int page = 1)
    {
        //var users = context.Users.AsQueryable();
        var users = context.Users
            .Where(x => x.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            users = users.Where(x =>
                x.FullName.Contains(searchString) ||
                x.Email.Contains(searchString) ||
                x.Role.Contains(searchString));
        }

        ViewData["CurrentFilter"] = searchString;
        int pageSize = 5;

        users = users.OrderBy(u => u.FullName);

        return View(await PaginatedList<CompanyInventory.Models.User>.CreateAsync(
            users.AsNoTracking(),
            page,
            pageSize));
    }
}