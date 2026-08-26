using CompanyInventory.Data;
using CompanyInventory.DTOs;
using CompanyInventory.Models;
using CompanyInventory.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CompanyInventory.Controllers;

[Authorize]
public class ProductController(ApplicationDbContext context) : Controller
{

    [HttpGet]
    public async Task<IActionResult> Index(string? searchString,int? categoryId,string? stockStatus,int page = 1)
    {
        ViewData["CurrentFilter"] = searchString;
        ViewBag.CategoryId = categoryId;
        ViewBag.StockStatus = stockStatus;
        int pageSize = 5;


        var products = context.Products
            .Where(p => p.IsActive)
            .Include(p => p.Category)
            .AsQueryable();

        //if (!string.IsNullOrWhiteSpace(searchString))
        //{
        //    products = products.Where(p =>
        //        p.ProductName.Contains(searchString) ||
        //        p.Category!.CategoryName.Contains(searchString));
        //}
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            products = products.Where(p =>
                p.ProductName.Contains(searchString) ||
                p.Category!.CategoryName.Contains(searchString) ||
                p.ProductId.ToString().Contains(searchString));
        }

        if (categoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == categoryId.Value);
        }
        if (!string.IsNullOrWhiteSpace(stockStatus))
        {
            switch (stockStatus)
            {
                case "In":
                    products = products.Where(p => p.Quantity > 10);
                    break;

                case "Low":
                    products = products.Where(p => p.Quantity > 0 && p.Quantity <= 10);
                    break;

                case "Out":
                    products = products.Where(p => p.Quantity == 0);
                    break;
            }
        }

        ViewBag.Categories = new SelectList( await context.Categories.Where(c => c.IsActive).ToListAsync(),
            "CategoryId",
            "CategoryName",
            categoryId);
        products = products.OrderBy(p => p.ProductName);

        return View(await PaginatedList<Product>.CreateAsync(
            products.AsNoTracking(),page,pageSize));
    }


    [HttpGet]
    public async Task<IActionResult> PartialIndex(string? searchString,int? categoryId,string? stockStatus,int page = 1)
    {
        ViewData["CurrentFilter"] = searchString;
        ViewBag.CategoryId = categoryId;
        ViewBag.StockStatus = stockStatus;

        int pageSize = 5;

        var products = context.Products
            .Where(p => p.IsActive)
            .Include(p => p.Category)
            .AsQueryable();

        //if (!string.IsNullOrWhiteSpace(searchString))
        //{
        //    products = products.Where(p =>
        //        p.ProductName.Contains(searchString) ||
        //        p.Category!.CategoryName.Contains(searchString));
        //}
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            products = products.Where(p =>
                p.ProductName.Contains(searchString) ||
                p.Category!.CategoryName.Contains(searchString) ||
                p.ProductId.ToString().Contains(searchString));
        }

        if (categoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(stockStatus))
        {
            switch (stockStatus)
            {
                case "In":
                    products = products.Where(p => p.Quantity > 10);
                    break;

                case "Low":
                    products = products.Where(p => p.Quantity > 0 && p.Quantity <= 10);
                    break;

                case "Out":
                    products = products.Where(p => p.Quantity == 0);
                    break;
            }
        }

        ViewBag.Categories = new SelectList(
            await context.Categories
                .Where(c => c.IsActive)
                .ToListAsync(),
            "CategoryId",
            "CategoryName",
            categoryId);

        products = products.OrderBy(p => p.ProductName);

        var model = await PaginatedList<Product>.CreateAsync(
            products.AsNoTracking(),
            page,
            pageSize);

        return PartialView("_ProductModule", model);
    }



    //[HttpGet]
    //public IActionResult Create()
    //{
    //    return RedirectToAction(nameof(Index));
    //}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductDto dto)
    {

        if(!ModelState.IsValid)
{
            return Json(new
            {
                success = false,
                message = "Please fill all required fields."
            });
        }

        var product = new Product
        {
            ProductId = dto.ProductId,
            ProductName = dto.ProductName,
            CategoryId = dto.CategoryId,
            MaterialCost = dto.MaterialCost,
            ServiceCost = dto.ServiceCost,
            Quantity = dto.Quantity,
            Description = dto.Description,
            CreatedBy = User.Identity?.Name
        };
        
        context.Products.Add(product);
        await context.SaveChangesAsync();

        TempData["Success"] = "Product added successfully.";

        return Json(new
        {
            success = true,
            message = "Product added successfully."
        });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        ViewBag.Categories = new SelectList(
            await context.Categories
            .Where(c => c.IsActive)
            .ToListAsync(),
            "CategoryId",
            "CategoryName",
            product.CategoryId);

        var dto = new ProductDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            CategoryId = product.CategoryId,
            MaterialCost = product.MaterialCost,
            ServiceCost = product.ServiceCost,
            Quantity = product.Quantity,
            Description = product.Description
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                message = "Please fill all required fields."
            });
        }

        var product = await context.Products.FindAsync(dto.ProductId);

        if (product == null)
            return NotFound();

        product.ProductName = dto.ProductName;
        product.CategoryId = dto.CategoryId;
        product.MaterialCost = dto.MaterialCost;
        product.ServiceCost = dto.ServiceCost;
        product.Quantity = dto.Quantity;
        product.Description = dto.Description;
        product.UpdatedOn = DateTime.Now;
        product.UpdatedBy = User.Identity?.Name;

        await context.SaveChangesAsync();

        return Json(new
        {
            success = true,
            message = "Product updated successfully."
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product != null)
        {
            product.IsActive = false;
            product.UpdatedOn = DateTime.Now;
            product.UpdatedBy = User.Identity?.Name;

            await context.SaveChangesAsync();
        }

        return Json(new
        {
            success = true,
            message = "Product deactivated successfully."
        });
    }
}