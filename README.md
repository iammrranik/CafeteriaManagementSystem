# 🍽️ Cafeteria Management System (ASP.NET Core)

<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0:11998e,100:38ef7d&height=200&section=header&text=Cafeteria%20Management&fontSize=40&fontColor=ffffff&animation=fadeIn"/>
</p>

<p align="center"> <a href="https://github.com/iammrranik"> <img src="https://img.shields.io/badge/_Click_Here_to_Explore_My_GitHub_Profile-181717?style=for-the-badge&logo=github&logoColor=white"/> </a> </p>


---


## ✨ Status
🚧 **Completed**  
🧠 Built with .NET 10.0 and .NET Core 3.1  
🎮 Uses [Entity Framework Core](https://github.com/dotnet/efcore)  
🎯 Designed for Final Evaluation  

---


## 🍽️ System Overview

This is a web-based Cafeteria Management System built following N-Tier architecture principles. The application allows customers to browse menu items, order meals, recharge their wallets and track their booking history. Administrators can manage user roles, audit logs, inspect financial records and process bulk cancellation refunds.

---

## 🔥 Features

- 🧩 N-Tier Architecture (OOP, separate layers for presentation, business logic and data access)
- ⚡ Advanced Features (personalized recommended menus, bulk cancellation and automatic refund mechanisms)
- 🖥️ User-Friendly Interface (simple Bootstrap forms, dynamic transaction tables and status grids)
- 📊 Financial Audit Oversight (deposit tracking, manual wallet balance adjustment and system log monitoring)
- 🔒 Session-Based Security (custom filters, login validation and role checks)
- 📁 Well-Structured Project Architecture

---


## 📌 Tech Stack

- .NET 10.0 / .NET Core 3.1
- Entity Framework Core
- SQL Server
- AutoMapper
- Bootstrap 5
- Git and GitHub
- Visual Studio
---


## 🗂️ Project Structure

```
├── CafeteriaManagementSystem.sln
├── README.md
├── App/
│   ├── Program.cs
│   ├── Validations/
│   │   └── UniqueIdCardNo.cs
│   ├── AuthFilters/
│   │   ├── AdminAccess.cs
│   │   └── CustomerAccess.cs
│   ├── Controllers/
│   │   ├── AdminController.cs
│   │   ├── AuthController.cs
│   │   └── CustomerController.cs
│   └── Views/
│       ├── Shared/
│       │   └── _Layout.cshtml
│       ├── Auth/
│       │   ├── Login.cshtml
│       │   └── Registration.cshtml
│       ├── Admin/
│       │   ├── Dashboard.cshtml
│       │   ├── Users.cshtml
│       │   ├── Bookings.cshtml
│       │   └── MenuItems.cshtml
│       └── Customer/
│           ├── Index.cshtml
│           ├── MyBookings.cshtml
│           └── Recharge.cshtml
├── BLL/
│   ├── MapperConfig.cs
│   ├── Services/
│   │   ├── UserService.cs
│   │   ├── MealBookingService.cs
│   │   └── MenuItemService.cs
│   └── DTOs/
│       ├── UserDTO.cs
│       ├── MealBookingDTO.cs
│       └── MenuItemDTO.cs
├── DAL/
│   ├── db.sql
│   ├── implementation.md
│   ├── EF/
│   │   └── CafeteriaDbContext.cs
│   └── Repos/
│       ├── UserRepo.cs
│       ├── MealBookingRepo.cs
│       └── MenuItemRepo.cs
```

---

## 🚦 How to Run

1. Initialize database: Run [db.sql](file:///f:/Projects/CafeteriaManagementSystem/DAL/db.sql) in SQL Server to create the tables and seed default data.
2. Open `CafeteriaManagementSystem.sln` in Visual Studio.
3. Restore NuGet dependencies and set `App` as the Startup Project.
4. Run the project:
  ```bash
  dotnet run --project App
  ```

---

## 📝 Design

The web application is designed using structured layered principles:

- **Routing & Session**: Managed in `App/Program.cs` and authorization filters, handling user context and access rules.
- **DTO Separation**: User input is bound to validation-annotated DTOs to separate the user interface from database structures.
- **Service Workflows**: Service classes in `BLL/Services` contain logical conditions (e.g. validating balances and stock counts).
- **Audit Logging**: Sensitive operations like profile modifications and user terminations are logged automatically.
- **Cascade Safe Deletions**: Associated system logs are automatically cleared prior to deleting user accounts to prevent database crashes.

---

## 📦 Requirements

- .NET Core SDK / .NET 10.0 SDK
- MS SQL Server

Install dependencies:
```bash
dotnet restore
```

---

## 🏆 Credits

Developed by [iammrranik](https://github.com/iammrranik) for an academic final project.

<p align="center"> <img src="https://capsule-render.vercel.app/api?type=rect&color=0:11998e,100:38ef7d&height=4" width="80%"/> </p>
