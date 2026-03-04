# Hospitio API Solution

Welcome to the **Hospitio API** project. This is a comprehensive, multi-tiered .NET solution designed to power the Hospitio backend services, featuring a clean architecture approach, background processing, and automated TypeScript model generation for frontend clients.

## 🏗️ Solution Architecture

The solution is divided into several purpose-built projects to maintain separation of concerns:

- **`HospitioApi`** 
  The main ASP.NET Core Web API project containing all the HTTP endpoints, minimal APIs/Controllers, MediatR handlers, and Swagger documentation.
  
- **`HospitioApi.Core`**
  The heart of the application. Contains domain entities, core business logic, interfaces, enumerations, and MediatR requests/responses.

- **`HospitioApi.Data`**
  The data access layer. Implements Entity Framework Core `DbContext`, database structures, and repository implementations.

- **`HospitioApi.Shared`**
  Shared utilities, cross-cutting concerns, constants, and common helpers used across the entire solution.

- **`Hospitio.BackGroundService`**
  A dedicated background worker service handling asynchronous tasks such as external alerts, SMS (Vonage), email sending, and message queue consumption (RabbitMQ).

- **`HospitioApi.DtoToTs`**
  A specialized automation tool that builds after compilation to convert C# Data Transfer Objects (DTOs) directly into TypeScript interfaces for seamless frontend integration.

- **`HospitioApi.Test`**
  The automated testing suite ensuring business logic stability and data context behavior.

---

## 🚀 Getting Started

### Prerequisites

Ensure the following tools are installed on your machine:
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) (or newer, with `RollForward` enabled)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) / [VS Code](https://code.visualstudio.com/) / JetBrains Rider
- SQL Server / LocalDB (or relevant database engine specified in `appsettings.json`)
- RabbitMQ (If running the background service locally)

### Building and Running the Solution

1. Restore the packages:
   ```bash
   dotnet restore HospitioApi.sln
   ```
2. Build the solution:
   ```bash
   dotnet build HospitioApi.sln
   ```
3. Run the API:
   ```bash
   dotnet run --project HospitioApi/HospitioApi.csproj
   ```

*(Note: Ensure your `appsettings.json` connection strings and external service keys are correctly configured before running.)*

---

## 🛠️ Development Guidelines & Best Practices

Please adhere to the following critical guidelines when contributing to the codebase:

1. **Separation of Handlers**: 
   Create a strict, separate MediatR handler for every single operation (`Create`, `Update`, `Delete`, `GetAll`, and `GetByID`). Do not combine them.
2. **Asynchronous Operations**: 
   You **must** pass `CancellationToken` on every asynchronous method call (e.g., database queries, API calls) throughout the application to ensure requests can be safely cancelled.
3. **Model Validation**: 
   Implement basic required validation in the Validation files based on your current understanding. This will act as a baseline and can be adjusted later based on UI requirements and future discussions.
4. **Endpoint Testing**: 
   Before moving on to develop the next API feature, ensure you thoroughly check/test your recent API work to confirm that all required operation endpoints (from Point 1) function correctly.
5. **Consistent Status Codes**: 
   Ensure response HTTP status codes are consistent. Do not use different codes for the same logical response (e.g., Do not mix `AppStatusCodeError.Gone410` and `AppStatusCodeError.Conflict409` for a "Not Found" entity — standardizing error responses is crucial).
6. **Code Cleanliness**: 
   Clean up your files. Always remove unused `using` namespaces and dead code from your C# files before committing.