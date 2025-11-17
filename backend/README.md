# Rentix — Backend (.NET 8)

Backend API for a rental management system implemented with .NET 8 and C# 12. The project demonstrates clean architecture principles, CQRS with MediatR, and a layered design that prioritizes testability, maintainability and separation of concerns.

## Summary

Rentix is a modular backend that implements domain-driven design patterns and modern .NET practices. It focuses on managing properties, leases, tenants and file documents (lease contracts), using abstractions for persistence and file storage.

## Key skills demonstrated

- Designing a clean, layered architecture (Domain / Application / Infrastructure / API).
- Implementing CQRS with MediatR (commands, queries, handlers) to separate write and read concerns.
- Applying validation and cross-cutting concerns through pipeline behaviors (FluentValidation + MediatR behaviors).
- Building testable services and handlers with dependency injection and abstraction interfaces.
- Handling file uploads via `IFormFile`, streaming to an `IFileStorageService` abstraction, and separating metadata from file storage.
- Mapping domain entities to DTOs using factory methods (`FromEntity`) to decouple persistence models from API contracts.
- Writing unit and integration tests to validate business logic and API scenarios.

## Architecture & patterns

- Clean / Onion Architecture: clear separation between Domain, Application, Infrastructure and API layers.
- CQRS + MediatR: commands and queries routed to handlers to keep concerns modular.
- DDD-friendly entities and factories: domain models encapsulate creation logic.
- Validation Behavior: cross-cutting validation is enforced before handler execution.
- File Storage Strategy: `IFileStorageService` allows swapping storage providers (local FS, cloud blobs).
- Global exception middleware: centralizes error handling and consistent API error responses.

## Technologies & libraries

- .NET 8, C# 12
- ASP.NET Core Web API
- MediatR (mediator pattern, CQRS)
- Entity Framework Core (persistence)
- FluentValidation (request/command validation)
- ILogger (structured logging)
- xUnit (unit & integration tests)

## Repository structure (high level)

- `Rentix.Domain` — domain entities, value objects, domain logic
- `Rentix.Application` — application services, commands/queries, DTOs, validators, behaviors
- `Rentix.Infrastructure` — EF Core persistence, file storage implementations, external integrations
- `Rentix.API` — controllers, model binding (`IFormFile`), middleware, composition root
- `Rentix.Tests` — unit and integration tests covering handlers and API flows

## Key features implemented

- Property and lease management: create/read operations for properties and leases.
- Tenant management associated with leases.
- Lease document upload and durable storage via an abstract `IFileStorageService`.
- DTO mapping and API contracts for safe exposure of domain data.
- Validation and error handling applied consistently through the request pipeline.
- Integration tests exercising API endpoints and file upload flows.

## Tests & quality

- Unit tests for command handlers, validators and core domain logic.
- Integration tests using a custom `WebApplicationFactory` to validate end-to-end scenarios (including multipart file uploads).
- CI-friendly structure: tests and build can be executed with `dotnet test` / `dotnet build` in a CI pipeline.

## Ideas for future improvements

- Add cloud-backed file providers (Azure Blob Storage, AWS S3) behind `IFileStorageService`.
- Add authentication/authorization (JWT, role-based policies) and secure file access URLs.
- Implement server-side streaming and range requests for large files to avoid high memory pressure.
- Add pagination, sorting and filtering to list endpoints and optimize queries for large datasets.
- Expand integration test coverage and add automated CI (GitHub Actions) to run builds and tests on PRs.