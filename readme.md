# Inventory Control API

This is the **backend API** for the Inventory Control system, developed with **C#**, **.NET 8**, and **SQL Server**, deployed with **AWS**. It provides a complete set of RESTful endpoints to manage products, sales, users, categories, suppliers, and inventory stock movements.

> 🔗 **Frontend Repository**: [Inventory Control Frontend](https://github.com/PauloDev367/Inventory_Control_Front)

## 🚀 Features

- ✅ Product CRUD operations with category management
- ✅ View and record stock movements
- ✅ Track product price and sales history
- ✅ User management and role-based access
- ✅ Authentication using ASP.NET Core Identity with JWT
- ✅ Sales registration and tracking
- ✅ Email notifications via SMTP
- ✅ Swagger UI for API testing
- ✅ Environment configuration using `.env` file

## 🛠️ Technologies Used

- **.NET 8**
- **C#**
- **SQL Server** (deployed via AWS RDS)
- **ASP.NET Core Identity**
- **Entity Framework Core**
- **JWT Authentication**
- **SMTP for email sending**
- **Swagger** for API documentation and testing
- **DotNetEnv** for loading environment variables from `.env` file

## 📁 Folder Structure

```bash
Inventory_Control/
│
├── Controllers/        # API endpoints
├── Data/               # Database context and seeders
├── Requests/               # Data Transfer Objects
├── Models/             # Entity models
├── Services/           # Business logic
├── Handles/            # Utility classes
├── Program.cs          # App startup configuration
└── appsettings.json    # Default environment configuration
```

## 🔐 Authentication

The API uses **ASP.NET Identity** for user authentication, with **JWT tokens** for secure access to protected routes. Users must log in and include the token in the `Authorization` header as a Bearer token.

## 📦 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/PauloDev367/Inventory_Control.git
cd Inventory_Control
```

### 2. Create a `.env` file in the root

```env
ConnectionStrings__SqlServer=""
JwtOptions__Issuer=""
JwtOptions__Audience=""
JwtOptions__SecurityKey=""
JwtOptions__Expiration=

SmtpSettings__Host= ""
SmtpSettings__Port= 
SmtpSettings__EnableSsl= ""
SmtpSettings__Username= ""
SmtpSettings__Password= ""
SmtpSettings__ToEmailAlert= ""
```

### 3. Run database migrations

Make sure your connection string is valid and your SQL Server instance is running (e.g., in AWS RDS).

```bash
dotnet ef database update
```

### 4. Run the API

```bash
dotnet run
```

### 5. Access Swagger

Navigate to:  
[http://localhost:5191/swagger](http://localhost:5191/swagger)

Use the Swagger interface to explore and test the available endpoints.

## 📬 Email Support

Emails (e.g., for notifications or password recovery) are sent using SMTP. Configuration is handled via `.env` and injected using a dedicated service.

## 🌐 Deployment

The API can be deployed to any environment supporting .NET 8. For database hosting, we recommend **AWS RDS** (SQL Server edition).

## 📄 License

This project is licensed under the MIT License.

---

**Developed with ❤️ by [PauloDev367](https://github.com/PauloDev367)**

