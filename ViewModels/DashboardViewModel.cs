using CompanyInventory.Models;

namespace CompanyInventory.ViewModels;

public class DashboardViewModel
{
    public int TotalProducts { get; set; }

    public int TotalCategories { get; set; }

    public int TotalTickets { get; set; }

    public int FourthTileCount { get; set; }

    public string FourthTileTitle { get; set; } = "";

    public string FourthTileIcon { get; set; } = "";

    public string FourthTileColor { get; set; } = "red";

    //public PaginatedList<Product> RecentProducts { get; set; } = default!;
}