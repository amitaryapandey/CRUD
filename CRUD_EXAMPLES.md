# CRUD Operations Examples

This document provides practical examples of how to use the Quotes API CRUD operations.

## Table of Contents
1. [Using cURL](#using-curl)
2. [Using Postman](#using-postman)
3. [Using C# HttpClient](#using-c-httpclient)
4. [Using JavaScript Fetch](#using-javascript-fetch)

---

## Using cURL

### 1. CREATE - Add a New Quote

```bash
curl -X POST https://localhost:5001/api/quotes \
  -H "Content-Type: application/json" \
  -d '{
    "title": "The only way to do great work is to love what you do",
    "author": "Steve Jobs",
    "description": "Success comes from passion and dedication"
  }'
```

**Expected Response:**
```json
{
  "id": 1,
  "title": "The only way to do great work is to love what you do",
  "author": "Steve Jobs",
  "description": "Success comes from passion and dedication"
}
```

---

### 2. READ - Get All Quotes

```bash
curl https://localhost:5001/api/quotes
```

**Expected Response:**
```json
[
  {
    "id": 1,
    "title": "The only way to do great work is to love what you do",
    "author": "Steve Jobs",
    "description": "Success comes from passion and dedication"
  }
]
```

---

### 3. READ - Get a Specific Quote

```bash
curl https://localhost:5001/api/quotes/1
```

**Expected Response:**
```json
{
  "id": 1,
  "title": "The only way to do great work is to love what you do",
  "author": "Steve Jobs",
  "description": "Success comes from passion and dedication"
}
```

---

### 4. UPDATE - Modify a Quote

```bash
curl -X PUT https://localhost:5001/api/quotes/1 \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Innovation distinguishes between a leader and a follower",
    "author": "Steve Jobs",
    "description": "Updated description"
  }'
```

**Expected Response:** 204 No Content (empty body)

---

### 5. DELETE - Remove a Quote

```bash
curl -X DELETE https://localhost:5001/api/quotes/1
```

**Expected Response:** 204 No Content (empty body)

---

## Using Postman

### Setup

1. Open Postman
2. Create a new collection called "Quotes API"
3. Set the base URL: `https://localhost:5001/api/quotes`

### 1. CREATE Request

- **Method:** POST
- **URL:** `{{base_url}}/quotes`
- **Headers:** 
  - Content-Type: application/json
- **Body (raw JSON):**
```json
{
  "title": "The only way to do great work is to love what you do",
  "author": "Steve Jobs",
  "description": "Success comes from passion and dedication"
}
```

### 2. READ All Quotes

- **Method:** GET
- **URL:** `{{base_url}}/quotes`
- **Headers:** None required
- **Body:** None

### 3. READ Single Quote

- **Method:** GET
- **URL:** `{{base_url}}/quotes/1`
- **Headers:** None required
- **Body:** None

### 4. UPDATE Quote

- **Method:** PUT
- **URL:** `{{base_url}}/quotes/1`
- **Headers:** 
  - Content-Type: application/json
- **Body (raw JSON):**
```json
{
  "title": "Updated Title",
  "author": "Updated Author",
  "description": "Updated Description"
}
```

### 5. DELETE Quote

- **Method:** DELETE
- **URL:** `{{base_url}}/quotes/1`
- **Headers:** None required
- **Body:** None

---

## Using C# HttpClient

### Example: Complete CRUD Operations

```csharp
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class QuoteApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://localhost:5001/api/quotes";

    public QuoteApiClient()
    {
        _httpClient = new HttpClient();
    }

    // CREATE
    public async Task<string> CreateQuoteAsync(string title, string author, string description)
    {
        var quote = new
        {
            title = title,
            author = author,
            description = description
        };

        var json = JsonSerializer.Serialize(quote);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(BaseUrl, content);
        return await response.Content.ReadAsStringAsync();
    }

    // READ ALL
    public async Task<string> GetAllQuotesAsync()
    {
        var response = await _httpClient.GetAsync(BaseUrl);
        return await response.Content.ReadAsStringAsync();
    }

    // READ BY ID
    public async Task<string> GetQuoteByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
        return await response.Content.ReadAsStringAsync();
    }

    // UPDATE
    public async Task<string> UpdateQuoteAsync(int id, string title, string author, string description)
    {
        var quote = new
        {
            title = title,
            author = author,
            description = description
        };

        var json = JsonSerializer.Serialize(quote);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{BaseUrl}/{id}", content);
        return await response.Content.ReadAsStringAsync();
    }

    // DELETE
    public async Task<bool> DeleteQuoteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        return response.IsSuccessStatusCode;
    }
}

// Usage
class Program
{
    static async Task Main(string[] args)
    {
        var client = new QuoteApiClient();

        // Create
        var createResult = await client.CreateQuoteAsync(
            "The only way to do great work",
            "Steve Jobs",
            "Passion and dedication"
        );
        Console.WriteLine("Create: " + createResult);

        // Read All
        var allQuotes = await client.GetAllQuotesAsync();
        Console.WriteLine("All Quotes: " + allQuotes);

        // Read by ID
        var quote = await client.GetQuoteByIdAsync(1);
        Console.WriteLine("Quote by ID: " + quote);

        // Update
        var updateResult = await client.UpdateQuoteAsync(
            1,
            "Updated Title",
            "Updated Author",
            "Updated Description"
        );
        Console.WriteLine("Update: " + updateResult);

        // Delete
        var deleteResult = await client.DeleteQuoteAsync(1);
        Console.WriteLine("Delete: " + deleteResult);
    }
}
```

---

## Using JavaScript Fetch

### Example: Complete CRUD Operations

```javascript
const baseUrl = 'https://localhost:5001/api/quotes';

// CREATE
async function createQuote(title, author, description) {
    const response = await fetch(baseUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            title: title,
            author: author,
            description: description
        })
    });
    return await response.json();
}

// READ ALL
async function getAllQuotes() {
    const response = await fetch(baseUrl);
    return await response.json();
}

// READ BY ID
async function getQuoteById(id) {
    const response = await fetch(`${baseUrl}/${id}`);
    return await response.json();
}

// UPDATE
async function updateQuote(id, title, author, description) {
    const response = await fetch(`${baseUrl}/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            title: title,
            author: author,
            description: description
        })
    });
    return response.ok;
}

// DELETE
async function deleteQuote(id) {
    const response = await fetch(`${baseUrl}/${id}`, {
        method: 'DELETE'
    });
    return response.ok;
}

// Usage Examples
(async () => {
    // Create
    const newQuote = await createQuote(
        'The only way to do great work',
        'Steve Jobs',
        'Passion and dedication'
    );
    console.log('Created:', newQuote);

    // Read All
    const allQuotes = await getAllQuotes();
    console.log('All Quotes:', allQuotes);

    // Read by ID
    const quote = await getQuoteById(1);
    console.log('Quote:', quote);

    // Update
    const updated = await updateQuote(
        1,
        'Updated Title',
        'Updated Author',
        'Updated Description'
    );
    console.log('Updated:', updated);

    // Delete
    const deleted = await deleteQuote(1);
    console.log('Deleted:', deleted);
})();
```

---

## Error Handling Examples

### Handling Errors in C#

```csharp
try
{
    var response = await _httpClient.GetAsync($"{BaseUrl}/999");
    
    if (!response.IsSuccessStatusCode)
    {
        var errorContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
    }
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Request failed: {ex.Message}");
}
```

### Handling Errors in JavaScript

```javascript
async function getQuoteWithErrorHandling(id) {
    try {
        const response = await fetch(`${baseUrl}/${id}`);
        
        if (!response.ok) {
            const error = await response.json();
            console.error(`Error ${response.status}:`, error.message);
            return null;
        }
        
        return await response.json();
    } catch (error) {
        console.error('Fetch failed:', error);
        return null;
    }
}
```

---

## Status Codes Reference

| Code | Meaning | Operation |
|------|---------|-----------|
| 200 | OK | GET successful |
| 201 | Created | POST successful |
| 204 | No Content | PUT/DELETE successful |
| 400 | Bad Request | Invalid input |
| 404 | Not Found | Resource not found |
| 500 | Server Error | Internal error |

---

## Tips & Best Practices

1. **Always validate input** before sending to API
2. **Handle errors gracefully** in your client code
3. **Use appropriate HTTP methods** (GET, POST, PUT, DELETE)
4. **Check status codes** to determine success/failure
5. **Use HTTPS** in production (not just localhost)
6. **Implement retry logic** for failed requests
7. **Use async/await** for better code readability
8. **Log API responses** for debugging

---

**Last Updated:** 2024
