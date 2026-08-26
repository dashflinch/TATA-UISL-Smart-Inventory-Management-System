using CompanyInventory.Data;
using CompanyInventory.DTOs;
using CompanyInventory.Models;
using CompanyInventory.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyInventory.Controllers;

[Authorize]
public class CategoryController(ApplicationDbContext context) : Controller
{
    // ==========================================
    // CATEGORY INDEX
    // ==========================================

    [HttpGet]
    public async Task<IActionResult> Index(
        string? searchString,
        int page = 1)
    {
        ViewData["CurrentFilter"] = searchString;

        int pageSize = 5;

        var categories = context.Categories
            .Where(c => c.IsActive)
            .AsQueryable();

        //if (!string.IsNullOrWhiteSpace(searchString))
        //{
        //    categories = categories.Where(c =>
        //        c.CategoryName.Contains(searchString) ||
        //        (c.Description != null &&
        //         c.Description.Contains(searchString)));
        //}
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            categories = categories.Where(c =>
                c.CategoryName.Contains(searchString) ||
                (c.Description != null &&
                 c.Description.Contains(searchString)) ||
                c.CategoryId.ToString().Contains(searchString));
        }

        categories = categories.OrderBy(c => c.CategoryName);

        return View(await PaginatedList<Category>.CreateAsync(
            categories.AsNoTracking(),
            page,
            pageSize));
    }


    // ==========================================
    // PARTIAL INDEX
    // ==========================================

    [HttpGet]
    public async Task<IActionResult> PartialIndex(
        string? searchString,
        int page = 1)
    {
        ViewData["CurrentFilter"] = searchString;

        int pageSize = 5;
          
        var categories = context.Categories
            .Where(c => c.IsActive)
            .AsQueryable();

        //if (!string.IsNullOrWhiteSpace(searchString))
        //{
        //    categories = categories.Where(c =>
        //        c.CategoryName.Contains(searchString) ||
        //        (c.Description != null &&
        //         c.Description.Contains(searchString)));
        //}
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            categories = categories.Where(c =>
                c.CategoryName.Contains(searchString) ||
                (c.Description != null &&
                 c.Description.Contains(searchString)) ||
                c.CategoryId.ToString().Contains(searchString));
        }

        categories = categories.OrderBy(c => c.CategoryName);

        var model = await PaginatedList<Category>.CreateAsync(
            categories.AsNoTracking(),
            page,
            pageSize);

        return PartialView("_CategoryModule", model);
    }


    // ==========================================
    // CREATE CATEGORY
    // ==========================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryDto dto)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                message = "Please fill all required fields."
            });
        }

        var category = new Category
        {
            CategoryName = dto.CategoryName,
            Description = dto.Description,
            CreatedBy = User.Identity?.Name
        };

        context.Categories.Add(category);

        await context.SaveChangesAsync();

        TempData["Success"] = "Category added successfully.";

        return Json(new
        {
            success = true,
            message = "Category added successfully."
        });
    }


    // ==========================================
    // EDIT CATEGORY GET
    // ==========================================

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await context.Categories.FindAsync(id);

        if (category == null)
            return NotFound();

        var dto = new CategoryDto
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            Description = category.Description
        };

        return View(dto);
    }


    // ==========================================
    // EDIT CATEGORY POST
    // ==========================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryDto dto)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                message = "Please fill all required fields."
            });
        }

        var category = await context.Categories
            .FindAsync(dto.CategoryId);

        if (category == null)
            return NotFound();

        category.CategoryName = dto.CategoryName;
        category.Description = dto.Description;
        category.UpdatedOn = DateTime.Now;
        category.UpdatedBy = User.Identity?.Name;

        await context.SaveChangesAsync();

        return Json(new
        {
            success = true,
            message = "Category updated successfully."
        });
    }


    // ==========================================
    // DELETE CATEGORY
    // ==========================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await context.Categories.FindAsync(id);

        if (category != null)
        {
            category.IsActive = false;
            category.UpdatedOn = DateTime.Now;
            category.UpdatedBy = User.Identity?.Name;

            await context.SaveChangesAsync();
        }

        return Json(new
        {
            success = true,
            message = "Category deactivated successfully."
        });
    }
}






























//using CompanyInventory.Data;
//using CompanyInventory.DTOs;
//using CompanyInventory.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using CompanyInventory.ViewModels;

//namespace CompanyInventory.Controllers;

//[Authorize]
//public class CategoryController(ApplicationDbContext context) : Controller
//{
//    /

//    public async Task<IActionResult> Index(string searchString, int page = 1)
//    {
//        ViewData["CurrentFilter"] = searchString;

//        int pageSize = 5; 

//        var categories = context.Categories
//            .Where(c => c.IsActive)
//            .AsQueryable();

//        if (!string.IsNullOrWhiteSpace(searchString))
//        {
//            categories = categories.Where(c =>
//                c.CategoryName.Contains(searchString));
//        }

//        categories = categories.OrderBy(c => c.CategoryName);

//        return View(await PaginatedList<Category>.CreateAsync(
//            categories.AsNoTracking(),
//            page,
//            pageSize));
//    }

//    [HttpGet]
//    public async Task<IActionResult> PartialIndex(string searchString, int page = 1)
//    {
//        ViewData["CurrentFilter"] = searchString;

//        int pageSize = 5;

//        var categories = context.Categories
//            .Where(c => c.IsActive)
//            .AsQueryable();

//        if (!string.IsNullOrWhiteSpace(searchString))
//        {
//            categories = categories.Where(c =>
//                c.CategoryName.Contains(searchString));
//        }

//        categories = categories.OrderBy(c => c.CategoryName);

//        var model = await PaginatedList<Category>.CreateAsync(
//            categories.AsNoTracking(),
//            page,
//            pageSize);

//        return PartialView("_CategoryModule", model);
//    }

//    [HttpGet]
//    public IActionResult Create()
//    {
//        return View();
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Create(CategoryDto dto)
//    {
//        if (!ModelState.IsValid)
//            return View(dto);

//        var category = new Category
//        {
//            CategoryName = dto.CategoryName,
//            Description = dto.Description,
//            CreatedBy = User.Identity?.Name
//        };

//        context.Categories.Add(category);
//        await context.SaveChangesAsync();

//        TempData["Success"] = "Category added successfully.";

//        return RedirectToAction(nameof(Index));
//    }

//    [HttpGet]
//    public async Task<IActionResult> Edit(int id)
//    {
//        var category = await context.Categories.FindAsync(id);

//        if (category == null)
//            return NotFound();

//        var dto = new CategoryDto
//        {
//            CategoryId = category.CategoryId,
//            CategoryName = category.CategoryName,
//            Description = category.Description,

//        };

//        return View(dto);
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Edit(CategoryDto dto)
//    {
//        if (!ModelState.IsValid)
//        {
//            TempData["Error"] = "Please fill all required fields.";
//            return RedirectToAction(nameof(Index));
//        }

//        var category = await context.Categories.FindAsync(dto.CategoryId);

//        if (category == null)
//            return NotFound();

//        category.CategoryName = dto.CategoryName;
//        category.Description = dto.Description;
//        category.UpdatedOn = DateTime.Now;
//        category.UpdatedBy = User.Identity?.Name;

//        await context.SaveChangesAsync();

//        TempData["Success"] = "Category updated successfully.";

//        return RedirectToAction(nameof(Index));
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Delete(int id)
//    {
//        var category = await context.Categories.FindAsync(id);

//        if (category != null)
//        {
//            category.IsActive = false;
//            category.UpdatedOn = DateTime.Now;
//            category.UpdatedBy = User.Identity?.Name;

//            await context.SaveChangesAsync();
//            TempData["Success"] = "Category deactivated successfully.";
//        }

//        return RedirectToAction(nameof(Index));
//    }
//}