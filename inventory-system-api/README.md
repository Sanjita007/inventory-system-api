# Inventory System API

A lightweight RESTful API for inventory management built with .NET 8 (C# 12). This project implements core inventory features including product and depot management, units and taxes, sales and purchase invoices, and reporting. The codebase follows a clean architecture with repository and service layers and uses `FluentValidation` for request validation.

## Key features

- Product and product group management
- Depot (warehouse) management
- Unit and compound unit support
- Tax configuration
- Sales and Purchase invoices with detailed line items
- Reporting endpoints (inventory summaries, dashboard data)
- Input validation using `FluentValidation`
- Repository + Service pattern for separation of concerns

## Tech stack

- .NET 8 (C# 12)
- ASP.NET Core Web API
- FluentValidation
- Any relational database (configure via `appsettings.json`)

## Architecture overview

The project separates concerns into layers:
- Application: DTOs, validators and application-level models
- Infrastructure: repositories (data access) and services
- API: controllers and request/response handling

This structure makes the code easier to maintain and test.

## Getting started

Prerequisites
- .NET 8 SDK
- Visual Studio 2022/2026 or VS Code

Local setup

1. Clone the repository:
   `git clone https://github.com/Sanjita007/inventory-system-api.git`
2. Open the solution in Visual Studio or use the command line to navigate to the project folder.
3. Update the database connection string in `appsettings.json` (set your connection string).
4. Restore and build:
   `dotnet restore`
   `dotnet build`
5. Apply database migrations (if migrations are included) or create the database schema required by your environment.
6. Run the API:
   `dotnet run` (from the API project folder) or start via Visual Studio.

Example request

```
curl -X GET "https://localhost:5001/api/products" \
  -H "Authorization: Bearer <YOUR_JWT_TOKEN>"
```

## Validation

The project uses `FluentValidation` to validate incoming models. Validators are located in the `Application/Validator` folder.

## Tests

If the repository contains test projects, run them via:
`dotnet test`

## Adding this project to LinkedIn

Contact

- GitHub: https://github.com/Sanjita007

