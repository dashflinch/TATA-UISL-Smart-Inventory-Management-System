using CompanyInventory.Data;
using CompanyInventory.Models;
using CompanyInventory.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyInventory.Controllers;

[Authorize]
public class DashboardController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(int page = 1)
    {
        DashboardViewModel vm = new()
        {
            TotalProducts = await context.Products.CountAsync(x => x.IsActive),

            TotalCategories = await context.Categories.CountAsync(x => x.IsActive),

            TotalTickets = await context.Tickets.CountAsync(x => x.IsActive),

            FourthTileTitle = "Open Tickets",

            FourthTileCount = await context.Tickets.CountAsync(x =>
                x.IsActive && x.Status != "Resolved"),

            FourthTileIcon = "fa-circle-exclamation",

            FourthTileColor = "red"
        };
        ViewBag.LowStock = await context.Products.CountAsync(x => x.IsActive && x.Quantity < 10);

        ViewBag.OpenTickets = await context.Tickets.CountAsync(x => x.IsActive && x.Status != "Resolved");

        return View(vm);
    }
}