# Quotes API - CRUD Operations in .NET

This is a complete CRUD (Create, Read, Update, Delete) API built with ASP.NET Core 2.1 and Entity Framework Core.

## Project Structure

```
QuotesApi/
├── Controllers/
│   ├── QuotesController.cs      # API endpoints for CRUD operations
│   └── ValuesController.cs
├── Models/
│   └── Quote.cs                 # Quote model/entity
├── Data/
│   └── QuotesDbContext.cs       # Entity Framework DbContext
├── Program.cs                   # Application entry point
├── Startup.cs                   # Service configuration
├── appsettings.json             # Configuration settings
└── QuotesApi.csproj             # Project file
```

## Model: Quote

```csharp
public class Quote
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
}
```

## CRUD Operations

### 1. CREATE - Add a New Quote
**Endpoint:** `POST /api/quotes`

**Request Body:**
```json
{
  "title": "The only way to do great work",
  "author": "Steve Jobs",
  "description": "Passion and dedication lead to excellence"
}
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

**Error Response (400 Bad Request):**
```json
{
  "message": "Title is required"
}
```

---

### 2. READ - Get All Quotes
**Endpoint:** `GET /api/quotes`

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "title": "The only way to do great work",
    "author": "Steve Jobs",
    "description": "Passion and dedication lead to excellence"
  },
  {
    "id": 2,
    "title": "Innovation distinguishes",
    "author": "Steve Jobs",
    "description": "Leaders and followers"
  }
]
```

---

### 3. READ - Get a Specific Quote by ID
**Endpoint:** `GET /api/quotes/{id}`

**Example:** `GET /api/quotes/1`

**Response (200 OK):**
```json
{
  "id": 1,
  "title": "The only way to do great work",
  "author": "Steve Jobs",
  "description": "Passion and dedication lead to excellence"
}
```

**Error Response (404 Not Found):**
```json
{
  "message": "Quote with ID 999 not found"
}
```

---

### 4. UPDATE - Modify an Existing Quote
**Endpoint:** `PUT /api/quotes/{id}`

**Example:** `PUT /api/quotes/1`

**Request Body:**
```json
{
  "title": "Updated Title",
  "author": "Updated Author",
  "description": "Updated Description"
}
```

**Response (204 No Content)** - No body returned on success

**Error Response (404 Not Found):**
```json
{
  "message": "Quote with ID 999 not found"
}
```

---

### 5. DELETE - Remove a Quote
**Endpoint:** `DELETE /api/quotes/{id}`

**Example:** `DELETE /api/quotes/1`

**Response (204 No Content)** - No body returned on success

**Error Response (404 Not Found):**
```json
{
  "message": "Quote with ID 999 not found"
}
```

---

## Key Features

✅ **Complete CRUD Operations** - Create, Read, Update, Delete functionality
✅ **Error Handling** - Try-catch blocks with meaningful error messages
✅ **HTTP Status Codes** - Proper status codes (200, 201, 204, 400, 404, 500)
✅ **Validation** - Input validation for required fields
✅ **Entity Framework Core** - ORM for database operations
✅ **Dependency Injection** - DbContext injected via constructor
✅ **XML Documentation** - Detailed comments for API methods
✅ **Response Type Attributes** - ProducesResponseType for API documentation

## Database

The application uses **SQL Server LocalDB** with the following connection string:
```
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuotesDb;
```

The database is automatically created on first run via `EnsureCreated()` in Startup.cs.

## Running the Application

### Prerequisites
- .NET Core 2.1 SDK
- SQL Server LocalDB (or modify connection string for your database)

### Steps

1. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

2. **Build the project:**
   ```bash
   dotnet build
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

4. **Access the API:**
   - The API will be available at `https://localhost:5001/api/quotes`

## Testing with cURL

### Create a Quote
```bash
curl -X POST https://localhost:5001/api/quotes \
  -H "Content-Type: application/json" \
  -d '{"title":"Test Quote","author":"Test Author","description":"Test Description"}'
```

### Get All Quotes
```bash
curl https://localhost:5001/api/quotes
```

### Get a Specific Quote
```bash
curl https://localhost:5001/api/quotes/1
```

### Update a Quote
```bash
curl -X PUT https://localhost:5001/api/quotes/1 \
  -H "Content-Type: application/json" \
  -d '{"title":"Updated Title","author":"Updated Author","description":"Updated Description"}'
```

### Delete a Quote
```bash
curl -X DELETE https://localhost:5001/api/quotes/1
```

## Technologies Used

- **ASP.NET Core 2.1** - Web framework
- **Entity Framework Core** - ORM
- **SQL Server** - Database
- **C#** - Programming language

## Architecture

The application follows a layered architecture:

1. **Controllers Layer** - Handles HTTP requests/responses
2. **Models Layer** - Defines data entities
3. **Data Layer** - Contains DbContext for database operations

## Best Practices Implemented

- ✅ Separation of concerns
- ✅ Dependency injection
- ✅ Exception handling
- ✅ Proper HTTP status codes
- ✅ Input validation
- ✅ RESTful API design
- ✅ XML documentation comments

## Future Enhancements

- Add pagination for GET all quotes
- Implement async/await patterns
- Add logging framework
- Add authentication/authorization
- Add unit tests
- Add API versioning
- Add rate limiting
- Add caching

---

**Author:** CRUD API Example
**Version:** 1.0
**License:** MIT
