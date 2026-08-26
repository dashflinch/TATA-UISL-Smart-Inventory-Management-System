# 🏭 Tata UISL Smart Inventory Management System

A web-based **Smart Inventory Management and Assistance System** developed to streamline inventory operations, product management, user management, support requests, and inventory-related assistance.

The system provides a centralized platform where authorized users can manage products, categories, users, inventory records, support tickets, and other operations through a secure role-based system.

---

# 📁 Project Structure

```text
CompanyInventory/
│
├── Constants/
│
├── Controllers/
│   ├── AuthController.cs
│   ├── CategoryController.cs
│   ├── ChatbotController.cs
│   ├── DashboardController.cs
│   ├── HomeController.cs
│   ├── ProductController.cs
│   ├── TicketController.cs
│   └── UserController.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── DTOs/
│   ├── CategoryDto.cs
│   ├── LoginDto.cs
│   ├── ProductDto.cs
│   └── RegisterDto.cs
│
├── Helpers/
│
├── Interfaces/
│
├── Migrations/
│
├── Models/
│
├── Properties/
│
├── Services/
│
├── ViewModels/
│
├── Views/
│   │
│   ├── Dashboard/
│   │   └── Index.cshtml
│   │
│   ├── Home/
│   │   ├── Index.cshtml
│   │   └── Privacy.cshtml
│   │
│   ├── Product/
│   │   ├── ProductModule.cshtml
│   │   └── Index.cshtml
│   │
│   ├── Ticket/
│   │   ├── _TicketModule.cshtml
│   │   └── Index.cshtml
│   │
│   ├── User/
│   │   └── Index.cshtml
│   │
│   └── Shared/
│       ├── _Chatbot.cshtml
│       ├── _DashboardLayout.cshtml
│       ├── _Layout.cshtml
│       ├── _ValidationScriptsPartial.cshtml
│       └── Error.cshtml
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── other static assets
│
├── appsettings.example.json
├── CompanyInventory.csproj
├── Program.cs
└── README.md
````

---

# ⚙️ Setup Instructions

## Step 1 – Prerequisites

Make sure the following software is installed on your system:

* Visual Studio 2022 or later
* .NET SDK compatible with the project
* Microsoft SQL Server
* SQL Server Management Studio (SSMS)

---

## Step 2 – Clone the Repository

Clone the repository from GitHub or download the project files.

```bash
git clone https://github.com/your-username/TATA-UISL-Smart-Inventory-Management-System.git
```

Then open the project folder in Visual Studio.

---

## Step 3 – Configure the Database

The project uses **Microsoft SQL Server**.

Create a new file named:

```text
appsettings.json
```

You can use the provided:

```text
appsettings.example.json
```

as a reference.

Example configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=CompanyInventoryDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Replace:

```text
YOUR_SERVER_NAME
```

with your own SQL Server instance name.

For example:

```text
localhost
.\SQLEXPRESS
PC-NAME\SQLEXPRESS
```

---

## Step 4 – Restore NuGet Packages

In Visual Studio, restore the required NuGet packages.

Or run:

```bash
dotnet restore
```

The project uses packages required for:

* Entity Framework Core
* Microsoft SQL Server connectivity
* ASP.NET Core MVC
* Authentication and Authorization

---

## Step 5 – Create the Database

The project uses **Entity Framework Core Migrations**.

Run the following command:

```bash
dotnet ef database update
```

Or, in Visual Studio Package Manager Console:

```powershell
Update-Database
```

This will create the database and required tables based on the project's Entity Framework migrations.

---

## Step 6 – Run the Application

Run the project from Visual Studio using:

```text
Ctrl + F5
```

or click the **Run** button.

The application will open in your browser.

---

# 🚀 Feature Guide

## 1. Authentication

* User registration
* User login
* Secure authentication
* Role-based authorization
* Protected application pages

Users can access different parts of the system according to their assigned role and permissions.

---

## 2. Dashboard

The dashboard provides a centralized overview of the inventory management system.

It helps users quickly access important modules and inventory-related information.

---

## 3. Product Management

Users can manage products through the Product module.

The system allows users to:

* View available products
* Add products
* Update product information
* Manage product details
* Organize inventory records

---

## 4. Category Management

The system supports category-based organization of products.

Users can:

* Create categories
* View categories
* Update category information
* Organize products by category

---

## 5. User Management

Authorized administrators can manage system users.

This includes:

* Viewing users
* Managing user information
* Controlling user roles
* Maintaining user access within the system

---

## 6. Ticket Management

The ticket module allows users to manage inventory-related support requests.

Users can:

* Create tickets
* View submitted tickets
* Track ticket information
* Manage support-related requests

---

## 7. Inventory Assistance

The system provides assistance features to support users during inventory-related operations.

These features help users interact with the system and access required information more efficiently.

---

## 8. Integrated Chatbot

The application includes an integrated chatbot feature.

The chatbot provides an additional assistance interface for users and improves interaction with the system.

---

# 🗄️ Database Architecture

The project uses **Microsoft SQL Server** as its relational database.

**Entity Framework Core** is used as the ORM for database operations.

Entity Framework Core handles:

* Database connectivity
* Entity mapping
* CRUD operations
* Database relationships
* Entity Framework migrations
* Database schema updates

The database structure is based on the application's entity models and migrations.

---

# 🔐 Authentication and Security

The system includes authentication and authorization features to protect application functionality.

Security-related features include:

* User authentication
* Role-based authorization
* Protected controller actions
* Secure application access
* Separation of sensitive configuration data

Private configuration values are not included in the public repository.

The repository provides:

```text
appsettings.example.json
```

Users should create their own:

```text
appsettings.json
```

with their local database configuration and private settings.

---

# 🛠️ Technologies Used

| Layer                | Technology                       |
| -------------------- | -------------------------------- |
| Backend              | ASP.NET Core MVC                 |
| Programming Language | C#                               |
| Database             | Microsoft SQL Server             |
| ORM                  | Entity Framework Core            |
| Database Migrations  | Entity Framework Core Migrations |
| Frontend             | HTML5, CSS3, JavaScript          |
| UI Views             | Razor Views                      |
| Responsive Design    | Bootstrap                        |
| Authentication       | ASP.NET Core Authentication      |
| IDE                  | Visual Studio                    |
| Version Control      | Git and GitHub                   |

---

# 🎨 UI Theme

The application uses a modern and responsive web interface.

### Frontend Technologies

* HTML5
* CSS3
* JavaScript
* Bootstrap
* Razor Views

### Design Features

* Responsive layouts
* Dashboard-based navigation
* Structured management modules
* Interactive user interface
* Organized application components
* Integrated assistance features

---

# 📦 Main Project Modules

The main modules of the system include:

* Authentication Module
* Dashboard Module
* Product Management Module
* Category Management Module
* User Management Module
* Ticket Management Module
* Chatbot and Assistance Module

---

# 🧩 Architecture

The project follows the **ASP.NET Core MVC architecture**.

### Model

The Model layer represents application data and database entities.

### View

The View layer contains Razor pages responsible for presenting information to users.

### Controller

The Controller layer handles user requests and coordinates communication between the View, Services, and Data layers.

### Data Layer

The Data layer manages database communication using Entity Framework Core and Microsoft SQL Server.

---

# 📂 Important Configuration Files

## `Program.cs`

The main application entry point.

It is responsible for configuring:

* Application services
* Entity Framework Core
* Database connectivity
* Authentication
* Authorization
* Middleware
* Routing

---

## `ApplicationDbContext.cs`

Located inside the `Data` folder.

This class manages communication between the application and Microsoft SQL Server through Entity Framework Core.

---

## `appsettings.example.json`

Provides an example configuration structure for users setting up the project locally.

Private values should be added only to a local `appsettings.json` file.

---

# 👩‍💻 Author

**Sonali Dash**

B.Tech Student
Computer Science and Information Technology

---

# 🏢 Project Information

This project was developed as an internship project associated with **Tata Steel UISL**.

The system focuses on improving the organization and management of inventory-related operations through a centralized software-based solution.

---

# 📄 License

This project is intended for academic and internship demonstration purposes.

---

⭐ If you found this project useful, consider giving the repository a star!

