# ITI E-Commerce API

Production-oriented ASP.NET Core Web API for a modern e-commerce backend.

[![.NET](https://img.shields.io/badge/.NET-10-512BD4)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-5C2D91)](https://learn.microsoft.com/aspnet/core)
[![EF Core](https://img.shields.io/badge/EF_Core-SQL_Server-0C54C2)](https://learn.microsoft.com/ef/core)

---

## Overview

This project provides a complete backend for e-commerce scenarios with:

- JWT Authentication and role-based authorization (`Admin`, `User`)
- Catalog management (Categories, Products)
- Cart management per authenticated user
- Order placement and order history
- File upload support for product/category images
- Validation using FluentValidation
- Layered architecture: Controller → Service → Repository → Database

---

## Solution Structure

```text
ITI_ECommerceApi/
├─ Ecommerce.API/        # Presentation layer (controllers, startup, API config)
├─ Ecommerce.BLL/        # Business logic, services, DTOs, validators, mappings
├─ Ecommerce.DAL/        # EF Core context, entities, configurations, repositories
└─ Common/               # Shared result/error contracts and settings
```

---

## Tech Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core + SQL Server
- ASP.NET Core Identity
- JWT Bearer Authentication
- AutoMapper
- FluentValidation
- Scalar / OpenAPI

---

## Key Features

### Authentication & Authorization
- Register and login endpoints
- JWT token generation with user role claims
- Role-based endpoint protection (`[Authorize(Roles = "Admin")]`)

### Product & Category Management
- CRUD operations with image upload support
- Pagination and filtering for products
- Unique category name enforcement

### Cart & Orders
- Add, update, remove cart items
- Clear cart
- Place order from cart
- Track user order history and retrieve order details

---

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server (LocalDB or full SQL Server)
- Visual Studio 2022/2026+ (recommended)

### 1) Clone

```bash
git clone https://github.com/omargibreel/ITI_ECommerceAPI
cd ITI_ECommerceAPI
```

### 2) Configure settings

Set your connection string and JWT settings in:

- `Ecommerce.API/appsettings.json`
- `Ecommerce.API/appsettings.Development.json`

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ITI_ECommerceDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "Key": "your-very-strong-secret-key",
    "Issuer": "ECommerceAPI",
    "Audience": "ECommerceClient",
    "ExpiryInDays": 7
  }
}
```

### 3) Restore and build

```bash
dotnet restore
dotnet build
```

### 4) Apply migrations

```bash
dotnet ef database update --project Ecommerce.DAL --startup-project Ecommerce.API
```

### 5) Run API

```bash
dotnet run --project Ecommerce.API
```

---

## API Documentation

In Development, OpenAPI/Scalar is available from the running API URL:

- `https://localhost:7264/scalar`
- `https://localhost:7264/openapi/v1.json`

---

## Seeded Roles & Users

At startup, the API seeds roles and default users:

- Roles: `Admin`, `User`
- Admin:
  - Email: `admin@gmail.com`
  - Password: `Admin123`
- User:
  - Email: `user@gmail.com`
  - Password: `User123`

> Change credentials in `Ecommerce.API/Extensions/IdentitySeederExtensions.cs` for non-local environments.

---

## Main Endpoints

### Auth
- `POST /api/Auth/register`
- `POST /api/Auth/login`

### Categories
- `GET /api/Categories`
- `GET /api/Categories/{id}`
- `POST /api/Categories` (Admin)
- `PUT /api/Categories/{id}` (Admin)
- `DELETE /api/Categories/{id}` (Admin)

### Products
- `GET /api/Products`
- `GET /api/Products/{id}`
- `POST /api/Products` (Admin)
- `PUT /api/Products/{id}` (Admin)
- `DELETE /api/Products/{id}` (Admin)

### Cart
- `GET /api/Cart`
- `POST /api/Cart/items`
- `PUT /api/Cart/items`
- `DELETE /api/Cart/items/{productId}`
- `DELETE /api/Cart`

### Orders
- `POST /api/Orders`
- `GET /api/Orders`
- `GET /api/Orders/{id}`

---

## Validation & Error Handling

- Request validation uses FluentValidation.
- Business/service responses are standardized using a `Result` pattern.
- Status codes are mapped to meaningful HTTP responses (`400`, `401`, `403`, `404`, `409`, `500`).

---

## Security Notes

- JWT authentication is enabled globally for protected endpoints.
- Role checks are enforced on admin operations.
- Password hashing is handled by ASP.NET Identity.
- Never use seeded credentials in production.

---

## Roadmap

- Global exception middleware with ProblemDetails
- Better conflict handling for delete operations with FK references
- Rate limiting and request throttling
- CI pipeline (build + tests)
- Integration tests for all API flows

---

## Author

**Omar Gibreel**

If this project helps you, consider starring the repository.
