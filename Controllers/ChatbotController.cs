using CompanyInventory.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace CompanyInventory.Controllers
{
    [Route("Chatbot")]
    public class ChatbotController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public ChatbotController(
            IWebHostEnvironment environment,
            ApplicationDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        [HttpPost("Ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.Message))
                {
                    return Json(new { reply = "Please enter a question." });
                }

                //string userQuestion = NormalizeText(request.Message);
                    string userQuestion = CorrectSpelling(NormalizeText(request.Message));

                // TICKET BY ID
                var ticketMatch = Regex.Match(
                    userQuestion,
                    @"(?:ticket|tickets)\s*(?:id|number)?\s*(\d+)"
                );

                if (ticketMatch.Success)
                {
                    int ticketId = int.Parse(ticketMatch.Groups[1].Value);

                    return Json(new
                    {
                        reply = await GetTicketDetails(ticketId)
                    });
                }


                // PRODUCT BY ID
                var productMatch = Regex.Match(
                    userQuestion,
                    @"(?:product|products)\s*(?:id|number)?\s*(\d+)"
                );

                if (productMatch.Success)
                {
                    int productId = int.Parse(productMatch.Groups[1].Value);

                    return Json(new
                    {
                        reply = await GetProductDetails(productId)
                    });
                }

                // ============================================
                // 1. LIVE DATABASE QUESTIONS
                // ============================================

                // DASHBOARD SUMMARY
                if (ContainsAny(userQuestion,
                    "dashboard summary",
                    "dashboard",
                    "system overview",
                    "statistics",
                    "inventory summary"))
                {
                    return Json(new
                    {
                        reply = await GetDashboardSummary()
                    });
                }

                // LOW / OUT OF STOCK PRODUCTS
                if (ContainsAny(userQuestion,
                    "low stock",
                    "low quantity",
                    "running out",
                    "out of stock"))
                {
                    return Json(new
                    {
                        reply = await GetLowStockProducts()
                    });
                }

                // PRODUCT COUNT
                if (ContainsAny(userQuestion,
                    "how many products",
                    "product count",
                    "number of products",
                    "total products"))
                {
                    var count = await _context.Products.CountAsync();

                    return Json(new
                    {
                        reply = $"📦 There are currently <strong>{count}</strong> products in the inventory."
                    });
                }

                // SHOW ALL PRODUCTS
                if (ContainsAny(userQuestion,
                    "show all products",
                    "show products",
                    "list products",
                    "list all products",
                    "view products",
                    "inventory list"))
                {
                    return Json(new
                    {
                        reply = await GetProducts()
                    });
                }

                // CATEGORY COUNT
                if (ContainsAny(userQuestion,
                    "how many categories",
                    "category count",
                    "number of categories",
                    "total categories"))
                {
                    var count = await _context.Categories.CountAsync();

                    return Json(new
                    {
                        reply = $"📂 There are currently <strong>{count}</strong> categories."
                    });
                }

                // SHOW ALL CATEGORIES
                if (ContainsAny(userQuestion,
                    "show all categories",
                    "show categories",
                    "list categories",
                    "list all categories",
                    "view categories"))
                {
                    return Json(new
                    {
                        reply = await GetCategories()
                    });
                }

                // MY TICKETS
                if (ContainsAny(userQuestion,
                    "my tickets",
                    "show my tickets",
                    "tickets i raised",
                    "my issues",
                    "my open tickets"))
                {
                    return Json(new
                    {
                        reply = await GetMyTickets()
                    });
                }

                // OPEN TICKETS
                if (ContainsAny(userQuestion,
                    "open tickets",
                    "show open tickets",
                    "pending tickets",
                    "unresolved tickets"))
                {
                    return Json(new
                    {
                        reply = await GetOpenTickets()
                    });
                }

                // ALL TICKETS
                if (ContainsAny(userQuestion,
                    "show all tickets",
                    "show tickets",
                    "list tickets",
                    "all tickets"))
                {
                    return Json(new
                    {
                        reply = await GetAllTickets()
                    });
                }

                // TICKET COUNT
                if (ContainsAny(userQuestion,
                    "how many tickets",
                    "ticket count",
                    "number of tickets",
                    "total tickets"))
                {
                    return Json(new
                    {
                        reply = await GetTicketCount()
                    });
                }

                // TICKET BY ID
                //var ticketMatch = Regex.Match(
                //    userQuestion,
                //    @"ticket\s*(?:id|number)?\s*(\d+)"
                //);
                //var ticketMatch = Regex.Match(
                //        userQuestion,
                //        @"(?:ticket|tickets)\s*(?:id|number)?\s*(\d+)"
                //    );

                //if (ticketMatch.Success)
                //{
                //    int ticketId = int.Parse(ticketMatch.Groups[1].Value);

                //    return Json(new
                //    {
                //        reply = await GetTicketDetails(ticketId)
                //    });
                //}


                // PRODUCT BY ID
                //var productMatch = Regex.Match(
                //    userQuestion,
                //    @"product\s*(?:id|number)?\s*(\d+)"
                //);
                //var productMatch = Regex.Match(
                //    userQuestion,
                //    @"(?:product|products)\s*(?:id|number)?\s*(\d+)"
                //);

                //if (productMatch.Success)
                //{
                //    int productId = int.Parse(productMatch.Groups[1].Value);

                //    return Json(new
                //    {
                //        reply = await GetProductDetails(productId)
                //    });
                //}

                


                // PRODUCTS BY CATEGORY
                var categoryProductMatch = Regex.Match(
                    userQuestion,
                    @"(?:products?|items?)\s+(?:in|from|under)\s+(.+)"
                );

                if (categoryProductMatch.Success)
                {
                    string categoryName = categoryProductMatch.Groups[1].Value.Trim();

                    return Json(new
                    {
                        reply = await GetProductsByCategory(categoryName)
                    });
                }

                // CATEGORY BY ID
                var categoryMatch = Regex.Match(
                    userQuestion,
                    @"category\s*(?:id|number)?\s*(\d+)"
                );

                if (categoryMatch.Success)
                {
                    int categoryId = int.Parse(categoryMatch.Groups[1].Value);

                    return Json(new
                    {
                        reply = await GetCategoryDetails(categoryId)
                    });
                }

                // TICKETS BY STATUS
                var statusMatch = Regex.Match(
                    userQuestion,
                    @"(?:show|list|get)?\s*(open|assigned|in progress|resolved|closed)\s+tickets?"
                );

                if (statusMatch.Success)
                {
                    string status = statusMatch.Groups[1].Value.Trim();

                    return Json(new
                    {
                        reply = await GetTicketsByStatus(status)
                    });
                }

                // TICKETS BY PRIORITY
                var priorityMatch = Regex.Match(
                    userQuestion,
                    @"(?:show|list|get)?\s*(low|medium|high|critical)\s+priority\s+tickets?"
                );

                if (priorityMatch.Success)
                {
                    string priority = priorityMatch.Groups[1].Value.Trim();

                    return Json(new
                    {
                        reply = await GetTicketsByPriority(priority)
                    });
                }

                // ============================================
                // 2. JSON KNOWLEDGE BASE FALLBACK
                // ============================================

                string filePath = Path.Combine(
                    _environment.WebRootPath,
                    "data",
                    "chatbotData.json");
                
                if (!System.IO.File.Exists(filePath))
                {
                    return Json(new
                    {
                        reply = "Knowledge base not found."
                    });
                }

                var json = await System.IO.File.ReadAllTextAsync(filePath);

                var data = JsonSerializer.Deserialize<List<ChatItem>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (data == null || data.Count == 0)
                {
                    return Json(new
                    {
                        reply = "Knowledge base is empty."
                    });
                }

                var words = userQuestion.Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries);

                ChatItem? bestMatch = null;
                int highestScore = 0;

                foreach (var item in data)
                {
                    int score = 0;

                    foreach (var keyword in item.Keywords)
                    {
                        string key = NormalizeText(keyword);

                        // Exact phrase match
                        if (userQuestion.Contains(key))
                        {
                            score += 10;
                        }

                        // Word-by-word match
                        foreach (var word in words)
                        {
                            if (key.Contains(word))
                            {
                                score++;
                            }
                        }
                    }

                    if (score > highestScore)
                    {
                        highestScore = score;
                        bestMatch = item;
                    }
                }

                if (bestMatch != null && highestScore > 0)
                {
                    return Json(new
                    {
                        reply = bestMatch.Answer
                    });
                }

                return Json(new
                {
                    reply = "Sorry, I couldn't find an answer to that. Try asking about Products, Categories, Tickets, Dashboard, Users, Login or Registration."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    reply = "An error occurred: " + ex.Message
                });
            }
        }

        // ============================================
        // HELPER METHODS
        // ============================================

        private static string NormalizeText(string text)
        {
            return Regex.Replace(
                text.ToLower().Trim(),
                @"[^\w\s]",
                ""
            );
        }


        private static int LevenshteinDistance(string source, string target)
        {
            int[,] distance = new int[source.Length + 1, target.Length + 1];

            for (int i = 0; i <= source.Length; i++)
                distance[i, 0] = i;

            for (int j = 0; j <= target.Length; j++)
                distance[0, j] = j;

            for (int i = 1; i <= source.Length; i++)
            {
                for (int j = 1; j <= target.Length; j++)
                {
                    int cost = source[i - 1] == target[j - 1] ? 0 : 1;

                    distance[i, j] = Math.Min(
                        Math.Min(
                            distance[i - 1, j] + 1,
                            distance[i, j - 1] + 1
                        ),
                        distance[i - 1, j - 1] + cost
                    );
                }
            }

            return distance[source.Length, target.Length];
        }

        private static string CorrectSpelling(string text)
        {
            string[] knownWords =
            {
        "product", "products",
        "category", "categories",
        "ticket", "tickets",
        "dashboard",
        "inventory",
        "stock",
        "quantity",
        "open",
        "closed",
        "resolved",
        "assigned",
        "progress",
        "priority",
        "high",
        "medium",
        "low",
        "critical",
        "show",
        "list",
        "details",
        "my",
        "all",
        "ticket",
        "tickets",
        "product",
        "products",
        "id",
        "number",
    };

            var words = text.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

            //for (int i = 0; i < words.Length; i++)
            //{
            //    string word = words[i];

            //    int bestDistance = int.MaxValue;
            //    string bestMatch = word;

            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];

                // Never correct numbers or very short words
                if (int.TryParse(word, out _) || word.Length <= 2)
                {
                    continue;
                }

                int bestDistance = int.MaxValue;
                string bestMatch = word;

                foreach (string knownWord in knownWords)
                {
                    int distance = LevenshteinDistance(word, knownWord);

                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestMatch = knownWord;
                    }
                }

                // Correct only small spelling mistakes
                if (bestDistance <= 2)
                {
                    words[i] = bestMatch;
                }
            }

            return string.Join(" ", words);
        }
        private static bool ContainsAny(string text, params string[] values)
        {
            return values.Any(value =>
                text.Contains(NormalizeText(value)));
        }

        // ============================================
        // PRODUCTS
        // ============================================

        private async Task<string> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedOn)
                .Take(10)
                .ToListAsync();

            if (!products.Any())
            {
                return "📦 No products found in the inventory.";
            }

            //string reply = "📦 <strong>Products</strong><br><br>";
            string reply = "📦 <strong>Latest 10 Products</strong><br><br>";

            foreach (var product in products)
            {
                reply +=
                    $"<strong>#{product.ProductId} - {product.ProductName}</strong><br>" +
                    $"📂 Category: {product.Category?.CategoryName ?? "N/A"}<br>" +
                    $"📦 Quantity: {product.Quantity}<br>" +
                    $"💰 Total Cost: ₹{product.TotalCost}<br><br>";
            }

            return reply;
        }

        private async Task<string> GetProductDetails(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return $"❌ Product #{productId} was not found.";
            }

            string status = product.IsActive
                ? "🟢 Active"
                : "🔴 Inactive";

            return
                $"📦 <strong>Product #{product.ProductId}</strong><br><br>" +
                $"<strong>Name:</strong> {product.ProductName}<br>" +
                $"<strong>Category:</strong> {product.Category?.CategoryName ?? "N/A"}<br>" +
                $"<strong>Material Cost:</strong> ₹{product.MaterialCost}<br>" +
                $"<strong>Service Cost:</strong> ₹{product.ServiceCost}<br>" +
                $"<strong>Total Cost:</strong> ₹{product.TotalCost}<br>" +
                $"<strong>Quantity:</strong> {product.Quantity}<br>" +
                $"<strong>Status:</strong> {status}<br>" +
                $"<strong>Description:</strong> {product.Description ?? "No description"}";
        }


        private async Task<string> GetProductsByCategory(string categoryName)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c =>
                    c.CategoryName.ToLower() == categoryName.ToLower());

            if (category == null)
            {
                return $"❌ Category <strong>{categoryName}</strong> was not found.";
            }

            var products = await _context.Products
                .Where(p => p.CategoryId == category.CategoryId)
                .OrderByDescending(p => p.CreatedOn)
                .ToListAsync();

            if (!products.Any())
            {
                return $"📂 No products found in <strong>{category.CategoryName}</strong>.";
            }

            string reply =
                $"📂 <strong>Products in {category.CategoryName} ({products.Count})</strong><br><br>";

            foreach (var product in products)
            {
                reply +=
                    $"📦 <strong>#{product.ProductId} - {product.ProductName}</strong><br>" +
                    $"📦 Quantity: {product.Quantity}<br>" +
                    $"💰 Total Cost: ₹{product.TotalCost}<br><br>";
            }

            return reply;
        }

        private async Task<string> GetCategoryDetails(int categoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            if (category == null)
            {
                return $"❌ Category #{categoryId} was not found.";
            }

            int productCount = await _context.Products
                .CountAsync(p => p.CategoryId == category.CategoryId);

            string status = category.IsActive
                ? "🟢 Active"
                : "🔴 Inactive";

            return
                $"📂 <strong>Category #{category.CategoryId}</strong><br><br>" +
                $"<strong>Name:</strong> {category.CategoryName}<br>" +
                $"<strong>Description:</strong> {category.Description ?? "No description"}<br>" +
                $"<strong>Products:</strong> {productCount}<br>" +
                $"<strong>Status:</strong> {status}";
        }
        private async Task<string> GetLowStockProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Quantity <= 5)
                .OrderBy(p => p.Quantity)
                .ToListAsync();

            if (!products.Any())
            {
                return "✅ Great! No low-stock products were found.";
            }

            string reply =
                "⚠️ <strong>Low Stock Products</strong><br><br>";

            foreach (var product in products)
            {
                reply +=
                    $"<strong>{product.ProductName}</strong><br>" +
                    $"📂 Category: {product.Category?.CategoryName ?? "N/A"}<br>" +
                    $"📦 Quantity Left: <strong>{product.Quantity}</strong><br><br>";
            }

            return reply;
        }

        // ============================================
        // CATEGORIES
        // ============================================

        private async Task<string> GetCategories()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            if (!categories.Any())
            {
                return "📂 No categories found.";
            }

            string reply =
                $"📂 <strong>Categories ({categories.Count})</strong><br><br>";

            foreach (var category in categories)
            {
                string status =
                    category.IsActive ? "🟢 Active" : "🔴 Inactive";

                reply +=
                    $"<strong>#{category.CategoryId} - {category.CategoryName}</strong><br>" +
                    $"{status}<br>";

                if (!string.IsNullOrWhiteSpace(category.Description))
                {
                    reply += $"📝 {category.Description}<br>";
                }

                reply += "<br>";
            }

            return reply;
        }

        // ============================================
        // TICKETS
        // ============================================


        private async Task<string> GetMyTickets()
        {
            string? userName = User.Identity?.Name;
            string? userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(userName) &&
                string.IsNullOrWhiteSpace(userEmail))
            {
                return "🔐 Please log in to view your tickets.";
            }

            var tickets = await _context.Tickets
                .Where(t =>
                    (!string.IsNullOrEmpty(userName) &&
                     t.CreatedBy == userName)
                    ||
                    (!string.IsNullOrEmpty(userEmail) &&
                     t.CreatedBy == userEmail)
                )
                .OrderByDescending(t => t.CreatedOn)
                .Take(10)
                .ToListAsync();

            if (!tickets.Any())
            {
                return "🎫 You have not raised any tickets yet.";
            }

            string reply =
                $"🎫 <strong>Your Recent Tickets ({tickets.Count})</strong><br><br>";

            foreach (var ticket in tickets)
            {
                reply +=
                    $"<strong>#{ticket.TicketId} - {ticket.Title}</strong><br>" +
                    $"🔥 Priority: {ticket.Priority}<br>" +
                    $"📌 Status: {ticket.Status}<br><br>";
            }

            return reply;
        }

        private async Task<string> GetOpenTickets()
        {
            var tickets = await _context.Tickets
                .Where(t => t.Status == "Open")
                .OrderByDescending(t => t.CreatedOn)
                .Take(10)
                .ToListAsync();

            if (!tickets.Any())
            {
                return "✅ There are currently no open tickets.";
            }

            string reply =
                $"🎫 <strong>Open Tickets ({tickets.Count})</strong><br><br>";

            foreach (var ticket in tickets)
            {
                reply +=
                    $"<strong>#{ticket.TicketId} - {ticket.Title}</strong><br>" +
                    $"🔥 Priority: {ticket.Priority}<br>" +
                    $"📌 Status: {ticket.Status}<br><br>";
            }

            return reply;
        }

        private async Task<string> GetTicketsByPriority(string priority)
        {
            var tickets = await _context.Tickets
                .Where(t => t.Priority.ToLower() == priority.ToLower())
                .OrderByDescending(t => t.CreatedOn)
                .Take(10)
                .ToListAsync();

            if (!tickets.Any())
            {
                return $"🎫 No <strong>{priority}</strong> priority tickets found.";
            }

            string reply =
                $"🎫 <strong>{priority} Priority Tickets ({tickets.Count})</strong><br><br>";

            foreach (var ticket in tickets)
            {
                reply +=
                    $"<strong>#{ticket.TicketId} - {ticket.Title}</strong><br>" +
                    $"📌 Status: {ticket.Status}<br>" +
                    $"🔥 Priority: {ticket.Priority}<br><br>";
            }

            return reply;
        }

        

        private async Task<string> GetAllTickets()
        {
            bool isAdmin = User.IsInRole("Admin");

            var query = _context.Tickets
                .OrderByDescending(t => t.CreatedOn)
                .AsQueryable();

            // If Admin, show all tickets
            if (isAdmin)
            {
                var adminTickets = await query
                    .Take(10)
                    .ToListAsync();

                if (!adminTickets.Any())
                {
                    return "🎫 No tickets found.";
                }

                string adminReply =
                    $"🎫 <strong>Recent Tickets ({adminTickets.Count})</strong><br><br>";

                foreach (var ticket in adminTickets)
                {
                    adminReply +=
                        $"<strong>#{ticket.TicketId} - {ticket.Title}</strong><br>" +
                        $"🔥 Priority: {ticket.Priority}<br>" +
                        $"📌 Status: {ticket.Status}<br><br>";
                }

                return adminReply;
            }

            // Normal user
            string? userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail))
            {
                return "🔐 Please log in to view tickets.";
            }

            var userTickets = await query
                .Where(t => t.CreatedBy == userEmail)
                .Take(10)
                .ToListAsync();

            if (!userTickets.Any())
            {
                return "🎫 You have not raised any tickets yet.";
            }

            string reply =
                $"🎫 <strong>Your Recent Tickets ({userTickets.Count})</strong><br><br>";

            foreach (var ticket in userTickets)
            {
                reply +=
                    $"<strong>#{ticket.TicketId} - {ticket.Title}</strong><br>" +
                    $"🔥 Priority: {ticket.Priority}<br>" +
                    $"📌 Status: {ticket.Status}<br><br>";
            }

            return reply;

        }

        private async Task<string> GetTicketCount()
        {
            var total = await _context.Tickets.CountAsync();
            var open = await _context.Tickets.CountAsync(t => t.Status == "Open");
            var resolved = await _context.Tickets.CountAsync(
                t => t.Status == "Resolved");

            return
                $"🎫 <strong>Ticket Summary</strong><br><br>" +
                $"Total Tickets: <strong>{total}</strong><br>" +
                $"Open: <strong>{open}</strong><br>" +
                $"Resolved: <strong>{resolved}</strong>";
        }

        private async Task<string> GetTicketDetails(int ticketId)
        {
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);

            if (ticket == null)
            {
                return $"❌ Ticket #{ticketId} was not found.";
            }

            return
                $"🎫 <strong>Ticket #{ticket.TicketId}</strong><br><br>" +
                $"<strong>Title:</strong> {ticket.Title}<br>" +
                $"<strong>Description:</strong> {ticket.Description}<br>" +
                $"<strong>Priority:</strong> {ticket.Priority}<br>" +
                $"<strong>Status:</strong> {ticket.Status}";
        }

        // ============================================
        // DASHBOARD
        // ============================================

        private async Task<string> GetDashboardSummary()
        {
            var totalProducts = await _context.Products.CountAsync();

            var totalCategories = await _context.Categories.CountAsync();

            var totalStock = await _context.Products
                .SumAsync(p => (int?)p.Quantity) ?? 0;

            var totalTickets = await _context.Tickets.CountAsync();

            var openTickets = await _context.Tickets
                .CountAsync(t => t.Status == "Open");

            var lowStockProducts = await _context.Products
                .CountAsync(p => p.Quantity <= 5);

            return
                $"📊 <strong>Company Inventory Summary</strong><br><br>" +
                $"📦 Total Products: <strong>{totalProducts}</strong><br>" +
                $"📂 Total Categories: <strong>{totalCategories}</strong><br>" +
                $"📦 Total Stock Quantity: <strong>{totalStock}</strong><br>" +
                $"🎫 Total Tickets: <strong>{totalTickets}</strong><br>" +
                $"🟠 Open Tickets: <strong>{openTickets}</strong><br>" +
                $"⚠️ Low Stock Products: <strong>{lowStockProducts}</strong>";
        }

        private async Task<string> GetTicketsByStatus(string status)
        {
            var tickets = await _context.Tickets
                .Where(t => t.Status.ToLower() == status.ToLower())
                .OrderByDescending(t => t.CreatedOn)
                .Take(10)
                .ToListAsync();

            if (!tickets.Any())
            {
                return $"🎫 No <strong>{status}</strong> tickets found.";
            }

            string reply =
                $"🎫 <strong>{status} Tickets ({tickets.Count})</strong><br><br>";

            foreach (var ticket in tickets)
            {
                reply +=
                    $"<strong>#{ticket.TicketId} - {ticket.Title}</strong><br>" +
                    $"🔥 Priority: {ticket.Priority}<br>" +
                    $"📌 Status: {ticket.Status}<br><br>";
            }

            return reply;
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = "";
    }

    public class ChatItem
    {
        [JsonPropertyName("keywords")]
        public List<string> Keywords { get; set; } = new();

        [JsonPropertyName("answer")]
        public string Answer { get; set; } = "";
    }
}






















































//using Microsoft.AspNetCore.Mvc;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Text.RegularExpressions;

//namespace CompanyInventory.Controllers
//{
//    [Route("Chatbot")]
//    public class ChatbotController : Controller
//    {
//        private readonly IWebHostEnvironment _environment;

//        public ChatbotController(IWebHostEnvironment environment)
//        {
//            _environment = environment;
//        }

//        [HttpPost("Ask")]
//        public IActionResult Ask([FromBody] ChatRequest request)
//        {
//            try
//            {
//                if (request == null || string.IsNullOrWhiteSpace(request.Message))
//                {
//                    return Json(new { reply = "Please enter a question." });
//                }

//                string filePath = Path.Combine(
//                    _environment.WebRootPath,
//                    "data",
//                    "chatbotData.json");

//                if (!System.IO.File.Exists(filePath))
//                {
//                    return Json(new { reply = "Knowledge base not found." });
//                }

//                var json = System.IO.File.ReadAllText(filePath);

//                var data = JsonSerializer.Deserialize<List<ChatItem>>(
//                    json,
//                    new JsonSerializerOptions
//                    {
//                        PropertyNameCaseInsensitive = true
//                    });

//                if (data == null || data.Count == 0)
//                {
//                    return Json(new
//                    {
//                        reply = "Knowledge base is empty."
//                    });
//                }

//                // Normalize user question
//                string userQuestion = Regex.Replace(
//                    request.Message.ToLower(),
//                    @"[^\w\s]",
//                    ""
//                );

//                var words = userQuestion.Split(
//                    ' ',
//                    StringSplitOptions.RemoveEmptyEntries);

//                ChatItem? bestMatch = null;
//                int highestScore = 0;

//                foreach (var item in data)
//                {
//                    int score = 0;

//                    foreach (var keyword in item.Keywords)
//                    {
//                        string key = keyword.ToLower();

//                        // Exact phrase match
//                        if (userQuestion.Contains(key))
//                        {
//                            score += 10;
//                        }

//                        // Word-by-word match
//                        foreach (var word in words)
//                        {
//                            if (key.Contains(word))
//                            {
//                                score++;
//                            }
//                        }
//                    }

//                    if (score > highestScore)
//                    {
//                        highestScore = score;
//                        bestMatch = item;
//                    }
//                }

//                if (bestMatch != null && highestScore > 0)
//                {
//                    return Json(new
//                    {
//                        reply = bestMatch.Answer
//                    });
//                }

//                return Json(new
//                {
//                    reply = "Sorry, I couldn't find an answer to that. Try asking about Products, Categories, Tickets, Dashboard, Users, Login or Registration."
//                });
//            }
//            catch (Exception ex)
//            {
//                return Json(new
//                {
//                    reply = "An error occurred: " + ex.Message
//                });
//            }
//        }
//    }

//    public class ChatRequest
//    {
//        public string Message { get; set; } = "";
//    }

//    public class ChatItem
//    {
//        [JsonPropertyName("keywords")]
//        public List<string> Keywords { get; set; } = new();

//        [JsonPropertyName("answer")]
//        public string Answer { get; set; } = "";
//    }
//}