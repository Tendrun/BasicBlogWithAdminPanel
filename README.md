# BasicBlogWithAdminPanel

A modern **ASP.NET Core MVC** blog platform with authentication, authorization, and an integrated admin panel.  
Developed as part of the **Platforma .NET** course at the **University of Zielona Góra**.

---

## 📖 Project Information

- **University**: Uniwersytet Zielonogórski  
- **Faculty**: Wydział Informatyki, Elektrotechniki i Automatyki  
- **Institute**: Instytut Sterowania i Systemów Informatycznych  
- **Course**: Platforma .NET  
- **Supervisor**: dr hab. inż. Marek Sawerwain, prof. UZ  
- **Project**: Blog z panelem administracyjnym w ASP.NET Core (wersja przedprodukcyjna)  
- **Authors**:  
  - Mikołaj Szczepaniak  
  - Jakub Siewierski  
- **Group**: 33-INF-SP-C  

---

## ✨ Features

- User registration and login with ASP.NET Identity  
- Role-based authorization (Admin, User)  
- **Admin dashboard** for managing posts, comments, and users  
- Multilingual support (English, Polish)  
- Entity Framework Core migrations  
- Responsive UI with Bootstrap  
- SQL Server database integration  
- AWS deployment ready  

---

## 🛠 Technologies

- **ASP.NET Core MVC** – application framework  
- **Entity Framework Core** – ORM and migrations  
- **Microsoft SQL Server (LocalDB)** – relational database  
- **Identity Framework** – authentication and authorization  
- **Razor Pages / .cshtml** – view engine  
- **C#** – business logic  
- **Visual Studio 2022** – development environment  
- **NuGet** – package manager  
- **AWS (planned)** – deployment platform  

---

## 📂 Project Structure

```
BasicBlogWithAdminPanel/
├── Controllers/        # MVC controllers
├── Models/             # Entity and view models
├── Views/              # Razor views
├── Areas/Identity/     # Identity scaffolding
├── Resources/          # Localization files
├── Migrations/         # EF Core migrations
├── wwwroot/            # Static assets
└── Dokumentacja/       # Documentation (build, diagrams, reports)
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)  
- Microsoft SQL Server  

### Setup

1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/BasicBlogWithAdminPanel.git
   cd BasicBlogWithAdminPanel
   ```

2. Restore dependencies:
   ```sh
   dotnet restore
   ```

3. Apply database migrations:
   ```sh
   dotnet ef database update
   ```

4. Run the application:
   ```sh
   dotnet run --project BasicBlogWithAdminPanel/BasicBlogWithAdminPanel.csproj
   ```

### Configuration

- Edit `BasicBlogWithAdminPanel/appsettings.json` for database connection and localization settings.  
- See [`Dokumentacja/How to build.txt`](Dokumentacja/How%20to%20build.txt) for migration commands and additional notes.  

---

## 🧪 Testing

- **Functional tests**: login, registration, posting, commenting, admin moderation.  
- **Validation tests**: empty fields, weak passwords, unauthorized edits.  
- **Future**: unit tests with xUnit, UI tests, performance checks under AWS deployment.  

---

## 📊 Deployment

- **Target environment**: Amazon Web Services (AWS EC2)  
- Planned tests after deployment:
  - Admin dashboard access  
  - User registration and login  
  - Post and comment CRUD operations  
  - Performance under concurrent requests  

---

## 👥 Authors’ Contribution

**Mikołaj Szczepaniak**  
- ASP.NET Core MVC implementation  
- Identity authentication and authorization  
- Data model (Post)  
- Admin panel with EF Core integration  

**Jakub Siewierski**  
- Frontend views (Razor Pages)  
- Localization and multilingual support  
- UI testing and validation scenarios  
- Documentation and deployment preparation  

---

## 📑 License

This project is for **educational purposes**.  
For external libraries, see their respective licenses in `wwwroot/lib/`.  

---
