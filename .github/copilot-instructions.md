# CoreKit Repository Instructions for GitHub Copilot

## Project Overview

CoreKit is a reusable enterprise-grade .NET application core/framework for rapidly building future business applications across multiple domains.

Examples:

* ERP
* CRM
* POS
* Hotel Management
* Restaurant Management
* Booking Systems
* Inventory / Warehouse
* Billing / Reservations

This repository is NOT a demo/sample/tutorial project.

Generate production-oriented maintainable enterprise code.

---

## Technology Stack

* .NET 10
* ASP.NET Core Minimal APIs
* Blazor WebAssembly Hosted
* PWA
* EF Core + SQL Server
* ASP.NET Core Identity
* Cookie Authentication (initial version)
* MediatR
* FluentValidation
* Finbuckle.MultiTenant
* Serilog

---

## Architecture

Follow:

* Modular Monolith Architecture
* Domain Driven Design (DDD)
* SOLID Principles
* CQRS Pattern
* Clean Architecture Concepts

---

## Solution Structure

```text
/src
  /CoreKit.AppHost.Server
  /CoreKit.AppHost.Client
  /CoreKit.AppHost.Contracts

  /BuildingBlocks
    /CoreKit.BuildingBlocks.Domain
    /CoreKit.BuildingBlocks.Application
    /CoreKit.BuildingBlocks.Infrastructure
    /CoreKit.BuildingBlocks.Presentation

  /Modules
    /Identity
    /Tenancy
    /[FutureModules]
```

---

## Layer Rules

### Domain Layer

Contains:

* Entities
* Value Objects
* Domain Events
* Domain Rules

Must NOT contain:

* Infrastructure logic
* EF Core code
* External dependencies

---

### Application Layer

Contains:

* Commands
* Queries
* DTOs
* Handlers
* Validation
* Use Cases

Must NOT contain:

* UI logic
* Infrastructure implementation details

---

### Infrastructure Layer

Contains:

* EF Core
* Identity integration
* Persistence
* External services

---

### Presentation Layer

Contains:

* Minimal API endpoints
* UI transport models

---

## Multi-Tenancy Rules

Use Database-Per-Tenant.

Requirements:

* Tenant Catalog database stores tenant metadata and connection strings.
* Each tenant has isolated database.
* Tenant must be resolved before auth and DbContext creation.
* All tenant DbContexts must be tenant-aware.

---

## API Rules

* Use Minimal APIs only.
* Do NOT generate MVC Controllers.
* Business operations go through unified RPC endpoint:

POST /api/rpc

Separate endpoints allowed only for:

* Auth
* Health
* System/Infrastructure

---

## Coding Rules

* Prefer composition over inheritance.
* Keep classes focused and small.
* Use extension methods for DI/service registration.
* Keep Program.cs clean.
* Avoid magic / implicit behavior.
* Favor explicit readable code.
* Use enterprise naming conventions.

---

## Important Restrictions

### Do NOT introduce unless explicitly requested:

* Generic Repository Pattern
* Unit Of Work wrappers over EF Core
* MVC Controllers
* Premature microservices patterns
* Unnecessary abstraction layers
* Reflection-heavy “magic” infrastructure

---

## When Generating Code

Always:

* Respect existing architecture
* Match project/folder conventions
* Suggest correct placement for new files
* Keep code production-ready
* Prefer maintainability over cleverness

---

## Output Expectations

When asked to implement something:

1. State which project/folder the code belongs to.
2. Explain impacted layers/modules.
3. Generate code consistent with architecture.
4. Mention if ADR or docs should be updated.
5. When adding important `.md` files or other working solution files, also add them to `CoreKit.sln` so they are visible in Solution Explorer.

---

## Long-Term Goal

CoreKit should evolve into a mature reusable internal platform for multiple business products.

All generated code should support long-term maintainability and extensibility.
