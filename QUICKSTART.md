# .NET CRUD API - Quick Start Guide

## Overview

This is a complete **CRUD (Create, Read, Update, Delete)** API built with **ASP.NET Core 2.1** and **Entity Framework Core**. It demonstrates best practices for building RESTful APIs in .NET.

## 🚀 Quick Start

### Prerequisites
- .NET Core 2.1 SDK or higher
- SQL Server LocalDB (or modify connection string)
- Visual Studio, VS Code, or any text editor

### Installation & Running

```bash
# 1. Restore NuGet packages
dotnet restore

# 2. Build the project
dotnet build

# 3. Run the application
dotnet run

# 4. API will be available at:
# https://localhost:5001/api/quotes
```

---

## 📁 Project Structure

```
QuotesApi/
│
├── Controllers/
│   ├── QuotesController.cs          ✅ API endpoints (CRUD operations)
│   └── ValuesController.cs
│
├── Models/
│   └── Quote.cs                     ✅ Data model
│
├── Data/
│   └── QuotesDbContext.cs           ✅ Entity Framework DbContext
│
├── Services/
│   ├── IQuoteService.cs             ✅ Service interface
│   └── QuoteService.cs              ✅ Service implementation
│
├── Program.cs                       ✅ Application entry point
├── Startup.cs                       ✅ Service configuration
├── appsettings.json                 ✅ Configuration
├── QuotesApi.csproj                 ✅ Project file
│
├── CRUD_OPERATIONS.md               📖 Detailed API documentation
├── CRUD_EXAMPLES.md                 📖 Usage examples
└── QUICKSTART.md                    📖 This file
```

---

## 🔧 API Endpoints

### Base URL
```
https://localhost:5001/api/quotes
```

### Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| **GET** | `/api/quotes` | Get all quotes |
| **GET** | `/api/quotes/{id}` | Get quote by ID |
| **POST** | `/api/quotes` | Create new quote |
| **PUT** | `/api/quotes/{id}` | Update quote |
| **DELETE** | `/api/quotes/{id}` | Delete quote |

---

## 📝 Quick Examples

### 1. Create a Quote (POST)

```bash
curl -X POST https://localhost:5001/api/quotes \
  -H "Content-Type: application/json" \
  -d '{
    "title": "The only way to do great work",
    "author": "Steve Jobs",
    "description": "Passion and dedication lead to excellence"
  }'
```

**Response (201 Created):**
```json
{
  "id": 1,
  "title": "The only way to do great work",
  "author": "Steve Jobs",
  "description": "Passion and dedication lead to excellence"
}
```

---

### 2. Get All Quotes (GET)

```bash
curl https://localhost:5001/api/quotes
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "title": "The only way to do great work",
    "author": "Steve Jobs",
    "description": "Passion and dedication lead to excellence"
  }
]
```

---

### 3. Get Single Quote (GET)

```bash
curl https://localhost:5001/api/quotes/1
```

**Response (200 OK):**
```json
{
  "id": 1,
  "title": "The only way to do great work",
  "author": "Steve Jobs",
  "description": "Passion and dedication lead to excellence"
}
```

---

### 4. Update Quote (PUT)

```bash
curl -X PUT https://localhost:5001/api/quotes/1 \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Updated Title",
    "author": "Updated Author",
    "description": "Updated Description"
  }'
```

**Response (204 No Content)** - Empty body

---

### 5. Delete Quote (DELETE)

```bash
curl -X DELETE https://localhost:5001/api/quotes/1
```

**Response (204 No Content)** - Empty body

---

## 🏗️ Architecture

### Layered Architecture

```
┌─────────────────────────────────────┐
│     HTTP Requests/Responses         │
├─────────────────────────────────────┤
│     Controllers Layer               │
│  (QuotesController.cs)              │
├─────────────────────────────────────┤
│     Services Layer                  │
│  (IQuoteService, QuoteService)      │
├─────────────────────────────────────┤
│     Data Layer                      │
│  (QuotesDbContext, Entity Framework)│
├─────────────────────────────────────┤
│     Database                        │
│  (SQL Server LocalDB)               │
└─────────────────────────────────────┘
```

---

## 🎯 Key Features

✅ **Complete CRUD Operations** - Create, Read, Update, Delete
✅ **Error Handling** - Try-catch with meaningful error messages
✅ **HTTP Status Codes** - Proper status codes (200, 201, 204, 400, 404, 500)
✅ **Input Validation** - Required field validation
✅ **Entity Framework Core** - ORM for database operations
✅ **Dependency Injection** - Built-in DI container
✅ **Service Layer** - Separation of concerns
✅ **XML Documentation** - Detailed API comments
✅ **RESTful Design** - Follows REST principles

---

## 📊 HTTP Status Codes

| Code | Meaning | When Used |
|------|---------|-----------|
| **200** | OK | GET successful |
| **201** | Created | POST successful |
| **204** | No Content | PUT/DELETE successful |
| **400** | Bad Request | Invalid input data |
| **404** | Not Found | Resource doesn't exist |
| **500** | Server Error | Internal server error |

---

## 🗄️ Database

### Connection String
```
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuotesDb;
```

### Quote Table Schema
```sql
CREATE TABLE Quotes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(MAX) NOT NULL,
    Author NVARCHAR(MAX),
    Description NVARCHAR(MAX)
);
```

### Auto-Creation
The database is automatically created on first run via `EnsureCreated()` in `Startup.cs`.

---

## 🧪 Testing

### Using Postman

1. Import the API endpoints into Postman
2. Set base URL: `https://localhost:5001/api/quotes`
3. Test each endpoint with sample data

### Using cURL

See [CRUD_EXAMPLES.md](CRUD_EXAMPLES.md) for complete cURL examples.

### Using C# HttpClient

See [CRUD_EXAMPLES.md](CRUD_EXAMPLES.md) for complete C# examples.

### Using JavaScript Fetch

See [CRUD_EXAMPLES.md](CRUD_EXAMPLES.md) for complete JavaScript examples.

---

## 📚 Documentation

- **[CRUD_OPERATIONS.md](CRUD_OPERATIONS.md)** - Detailed API documentation
- **[CRUD_EXAMPLES.md](CRUD_EXAMPLES.md)** - Practical usage examples
- **[QUICKSTART.md](QUICKSTART.md)** - This file

---

## 🔐 Security Considerations

For production deployment, consider:

- ✅ Add authentication (JWT, OAuth)
- ✅ Add authorization (roles, permissions)
- ✅ Use HTTPS (not just localhost)
- ✅ Add rate limiting
- ✅ Add input sanitization
- ✅ Add logging and monitoring
- ✅ Use environment variables for secrets
- ✅ Add CORS configuration

---

## 🚀 Future Enhancements

- [ ] Add pagination for GET all
- [ ] Implement async/await patterns
- [ ] Add logging framework (Serilog)
- [ ] Add authentication/authorization
- [ ] Add unit tests (xUnit)
- [ ] Add API versioning
- [ ] Add caching (Redis)
- [ ] Add rate limiting
- [ ] Add Swagger/OpenAPI documentation
- [ ] Add database migrations (EF Core Migrations)

---

## 🛠️ Technologies Used

- **ASP.NET Core 2.1** - Web framework
- **Entity Framework Core** - ORM
- **SQL Server LocalDB** - Database
- **C#** - Programming language
- **Dependency Injection** - Built-in DI

---

## 📖 Model Definition

```csharp
public class Quote
{
    public int Id { get; set; }              // Primary key
    public string Title { get; set; }        // Quote title (required)
    public string Author { get; set; }       // Quote author
    public string Description { get; set; }  // Quote description
}
```

---

## 🤝 Contributing

Feel free to fork, modify, and improve this project!

---

## 📄 License

MIT License - Feel free to use this project for learning and development.

---

## 📞 Support

For issues or questions:
1. Check the documentation files
2. Review the examples
3. Check the code comments

---

**Version:** 1.0  
**Last Updated:** 2024  
**Framework:** ASP.NET Core 2.1  
**Language:** C#
