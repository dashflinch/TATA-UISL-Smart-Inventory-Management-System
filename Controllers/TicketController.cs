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
public class TicketController(ApplicationDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? searchString,int? ticketId,DateTime? createdDate,string? issueCategory,string? status,string? priority,int page = 1)
    {
        ViewData["CurrentFilter"] = searchString;

        ViewBag.TicketId = ticketId;
        ViewBag.CreatedDate = createdDate?.ToString("yyyy-MM-dd");
        ViewBag.IssueCategory = issueCategory;
        ViewBag.Status = status;
        ViewBag.Priority = priority;
        int pageSize = 5;
        var tickets = context.Tickets
            .Include(t => t.Product)
            .Where(t => t.IsActive)
            .AsQueryable();

        

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            searchString = searchString.Trim();

            DateTime parsedDate;
            bool isDate = DateTime.TryParse(searchString, out parsedDate);

            tickets = tickets.Where(t =>

                t.Title.Contains(searchString) ||

                t.IssueCategory.Contains(searchString) ||

                t.IssueType.Contains(searchString) ||

                t.AffectedSystem.Contains(searchString) ||

                t.TicketId.ToString().Contains(searchString) ||

                (isDate &&
                 t.CreatedOn.Date == parsedDate.Date) ||

                (t.Product != null &&
                 t.Product.ProductName.Contains(searchString))

            );
        }

        if (ticketId.HasValue)
        {
            tickets = tickets.Where(t => t.TicketId == ticketId.Value);
        }

        if (createdDate.HasValue)
        {
            tickets = tickets.Where(t => t.CreatedOn.Date == createdDate.Value.Date);
        }

        if (!string.IsNullOrWhiteSpace(issueCategory))
        {
            tickets = tickets.Where(t => t.IssueCategory == issueCategory);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            tickets = tickets.Where(t => t.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(priority))
        {
            tickets = tickets.Where(t => t.Priority == priority);
        }

        ViewBag.Products = new SelectList(
            await context.Products.ToListAsync(),
            "ProductId",
            "ProductName");

        tickets = tickets.OrderByDescending(t => t.CreatedOn);


        ViewBag.TotalTickets = await context.Tickets.CountAsync();

        ViewBag.OpenTickets = await context.Tickets.CountAsync(t => t.Status == "Open");

        ViewBag.ResolvedTickets = await context.Tickets.CountAsync(t => t.Status == "Resolved");

        ViewBag.CriticalTickets = await context.Tickets.CountAsync(t => t.Priority == "Critical");


        return View(await PaginatedList<Ticket>.CreateAsync(
            tickets.AsNoTracking(),
            page,
            pageSize));
    }


    [HttpGet]
    public async Task<IActionResult> PartialIndex(
    string? searchString,
    int? ticketId,
    DateTime? createdDate,
    string? issueCategory,
    string? status,
    string? priority,
    int page = 1)
    {
        ViewData["CurrentFilter"] = searchString;

        ViewBag.TicketId = ticketId;
        ViewBag.CreatedDate = createdDate?.ToString("yyyy-MM-dd");
        ViewBag.IssueCategory = issueCategory;
        ViewBag.Status = status;
        ViewBag.Priority = priority;

        int pageSize = 5;

        var tickets = context.Tickets
    .Include(t => t.Product)
    .Where(t => t.IsActive)
    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            searchString = searchString.Trim();

            DateTime parsedDate;
            bool isDate = DateTime.TryParse(searchString, out parsedDate);

            tickets = tickets.Where(t =>

                t.Title.Contains(searchString) ||

                t.IssueCategory.Contains(searchString) ||

                t.IssueType.Contains(searchString) ||

                t.AffectedSystem.Contains(searchString) ||

                t.TicketId.ToString().Contains(searchString) ||

                (isDate &&
                 t.CreatedOn.Date == parsedDate.Date) ||

                (t.Product != null &&
                 t.Product.ProductName.Contains(searchString))
            );
        }

        if (ticketId.HasValue)
        {
            tickets = tickets.Where(t => t.TicketId == ticketId.Value);
        }

        if (createdDate.HasValue)
        {
            tickets = tickets.Where(t => t.CreatedOn.Date == createdDate.Value.Date);
        }

        if (!string.IsNullOrWhiteSpace(issueCategory))
        {
            tickets = tickets.Where(t => t.IssueCategory == issueCategory);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            tickets = tickets.Where(t => t.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(priority))
        {
            tickets = tickets.Where(t => t.Priority == priority);
        }

        tickets = tickets.OrderByDescending(t => t.CreatedOn);

        ViewBag.Products = new SelectList(
            await context.Products
                .Where(p => p.IsActive)
                .ToListAsync(),
            "ProductId",
            "ProductName");

        var model = await PaginatedList<Ticket>.CreateAsync(
            tickets.AsNoTracking(),
            page,
            pageSize);

        ViewBag.TotalTickets = await context.Tickets.CountAsync();

        ViewBag.OpenTickets = await context.Tickets.CountAsync(t => t.Status == "Open");

        ViewBag.ResolvedTickets = await context.Tickets.CountAsync(t => t.Status == "Resolved");

        ViewBag.CriticalTickets = await context.Tickets.CountAsync(t => t.Priority == "Critical");

        return PartialView("_TicketModule", model);
    }





    [HttpGet]
    public IActionResult Create()
    {
        return RedirectToAction(nameof(Index));
    }


    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create(TicketDto dto)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        ViewBag.Products = new SelectList(
    //            await context.Products.ToListAsync(),
    //            "ProductId",
    //            "ProductName",
    //            dto.ProductId);

    //        //return View(dto);
    //        TempData["Error"] = "Please fill all required fields.";
    //        return RedirectToAction(nameof(Index));
    //    }



    //    var ticket = new Ticket
    //    {
    //        Title = dto.Title,
    //        Description = dto.Description,

    //        IssueCategory = dto.IssueCategory,
    //        IssueType = dto.IssueType,
    //        AffectedSystem = dto.AffectedSystem,
    //        Impact = dto.Impact,

    //        ProductId = dto.ProductId,
    //        Priority = dto.Priority,

    //        Status = User.IsInRole("Admin")
    //    ? dto.Status
    //    : "Open",

    //        AssignedTo = User.IsInRole("Admin")
    //    ? dto.AssignedTo
    //    : null,

    //        ResolutionNotes = dto.ResolutionNotes,

    //        CreatedBy = User.Identity?.Name
    //    };

    //    context.Tickets.Add(ticket);
    //    await context.SaveChangesAsync();

    //    TempData["Success"] = "Ticket created successfully.";

    //    return RedirectToAction(nameof(Index));
    //}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TicketDto dto)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                message = "Please fill all required fields."
            });
        }

        var ticket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,

            IssueCategory = dto.IssueCategory,
            IssueType = dto.IssueType,
            AffectedSystem = dto.AffectedSystem,
            Impact = dto.Impact,

            ProductId = dto.ProductId,
            Priority = dto.Priority,

            Status = User.IsInRole("Admin")
                ? dto.Status
                : "Open",

            AssignedTo = User.IsInRole("Admin")
                ? dto.AssignedTo
                : null,

            ResolutionNotes = dto.ResolutionNotes,

            CreatedBy = User.Identity?.Name
        };

        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();

        return Json(new
        {
            success = true,
            message = "Ticket created successfully."
        });
    }



    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var ticket = await context.Tickets.FindAsync(id);

        if (ticket == null)
            return NotFound();

        ViewBag.Products = new SelectList(
            await context.Products.ToListAsync(),
            "ProductId",
            "ProductName",
            ticket.ProductId);

        //var dto = new TicketDto
        //{
        //    TicketId = ticket.TicketId,
        //    Title = ticket.Title,
        //    Description = ticket.Description,
        //    ProductId = ticket.ProductId,
        //    Priority = ticket.Priority,
        //    Status = ticket.Status,
        //    AssignedTo = ticket.AssignedTo
        //};
        var dto = new TicketDto
        {
            TicketId = ticket.TicketId,
            Title = ticket.Title,
            Description = ticket.Description,

            IssueCategory = ticket.IssueCategory,
            IssueType = ticket.IssueType,
            AffectedSystem = ticket.AffectedSystem,
            Impact = ticket.Impact,

            ProductId = ticket.ProductId,
            Priority = ticket.Priority,
            Status = ticket.Status,
            AssignedTo = ticket.AssignedTo,
            ResolutionNotes = ticket.ResolutionNotes
        };

        //return View(dto);
        return RedirectToAction(nameof(Index));
    }


    //[Authorize(Roles = "Admin")]
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(TicketDto dto)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        ViewBag.Products = new SelectList(
    //            await context.Products.ToListAsync(),
    //            "ProductId",
    //            "ProductName",
    //            dto.ProductId);

    //        TempData["Error"] = "Please fill all required fields.";
    //        return RedirectToAction(nameof(Index));
    //    }

    //    var ticket = await context.Tickets.FindAsync(dto.TicketId);

    //    if (ticket == null)
    //        return NotFound();

    //    ticket.Title = dto.Title;
    //    ticket.Description = dto.Description;

    //    ticket.IssueCategory = dto.IssueCategory;
    //    ticket.IssueType = dto.IssueType;
    //    ticket.AffectedSystem = dto.AffectedSystem;
    //    ticket.Impact = dto.Impact;

    //    ticket.ProductId = dto.ProductId;
    //    ticket.Priority = dto.Priority;
    //    ticket.Status = dto.Status;
    //    ticket.AssignedTo = dto.AssignedTo;
    //    ticket.ResolutionNotes = dto.ResolutionNotes;
    //    ticket.UpdatedOn = DateTime.Now;
    //    ticket.UpdatedBy = User.Identity?.Name;

    //    await context.SaveChangesAsync();

    //    TempData["Success"] = "Ticket updated successfully.";

    //    return RedirectToAction(nameof(Index));
    //}


    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TicketDto dto)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                message = "Please fill all required fields."
            });
        }

        var ticket = await context.Tickets.FindAsync(dto.TicketId);

        if (ticket == null)
        {
            return Json(new
            {
                success = false,
                message = "Ticket not found."
            });
        }

        ticket.Title = dto.Title;
        ticket.Description = dto.Description;

        ticket.IssueCategory = dto.IssueCategory;
        ticket.IssueType = dto.IssueType;
        ticket.AffectedSystem = dto.AffectedSystem;
        ticket.Impact = dto.Impact;

        ticket.ProductId = dto.ProductId;
        ticket.Priority = dto.Priority;
        ticket.Status = dto.Status;
        ticket.AssignedTo = dto.AssignedTo;
        ticket.ResolutionNotes = dto.ResolutionNotes;

        ticket.UpdatedOn = DateTime.Now;
        ticket.UpdatedBy = User.Identity?.Name;

        await context.SaveChangesAsync();

        return Json(new
        {
            success = true,
            message = "Ticket updated successfully."
        });
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var ticket = await context.Tickets.FindAsync(id);

        if (ticket == null)
        {
            return Json(new
            {
                success = false,
                message = "Ticket not found."
            });
        }

        ticket.IsActive = false;
        ticket.UpdatedOn = DateTime.Now;
        ticket.UpdatedBy = User.Identity?.Name;

        await context.SaveChangesAsync();

        return Json(new
        {
            success = true,
            message = "Ticket deleted successfully."
        });
    }


}
