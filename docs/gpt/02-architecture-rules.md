# CoreKit Architecture Rules

## Architectural Style

CoreKit follows:

* Modular Monolith Architecture
* Domain Driven Design
* SOLID Principles
* CQRS Pattern
* Clean Architecture Concepts

## Module Design Rules

Each business capability should be implemented as a module.

Each module may contain:

* Domain
* Application
* Infrastructure
* Presentation (optional)

Modules must:

* be independently extendable
* expose only necessary public contracts
* minimize coupling to other modules
* implement the shared module registration/startup contract

## Layer Responsibilities

### Domain

Contains:

* Entities
* Value Objects
* Domain Events
* Domain Rules
* Domain Services (only when appropriate)

Must NOT contain:

* Infrastructure concerns
* EF Core logic
* External dependencies

### Application

Contains:

* Commands
* Queries
* DTOs
* Handlers
* Validation
* Use Case Orchestration

Must NOT contain:

* Database implementation details
* UI logic

### Infrastructure

Contains:

* EF Core
* Identity integration
* External service integrations
* Persistence implementations

### Presentation

Contains:

* API endpoints
* UI-specific presentation models
* Mapping from transport layer

## API Rules

* Minimal APIs only
* No MVC Controllers unless explicitly justified
* Business operations routed through unified RPC endpoint
* Infrastructure/system endpoints may remain separate

## Coding Rules

* Prefer composition over inheritance
* Avoid generic repository pattern unless strongly justified
* Use DbContext directly where appropriate
* Keep Program.cs clean via extension methods
* Favor explicitness over magic
* Avoid premature abstractions

## Dependency Rules

Allowed direction:
Presentation -> Application -> Domain
Infrastructure -> Application/Domain

Forbidden:

* Domain referencing Infrastructure
* Application referencing Presentation
* Cross-module tight coupling
* UI components calling raw module transport strings directly when a module client exists

## Module Framework Rules

Each module should expose a single module entry point that implements the shared host contract.

Preferred pattern:

* `BuildingBlocks.Presentation` contains the shared module contract
* module `Presentation` project contains the concrete module class
* AppHost discovers/registers modules through a catalog, not hand-written ad hoc calls in `Program.cs`

When a module is used by the client:

* shared transport contracts live in `CoreKit.AppHost.Contracts`
* the client uses a module-specific client service
* transport details should stay out of page/component code when possible

## Naming Rules

* Clear, explicit, enterprise-oriented naming
* Avoid vague names like Manager, Helper, Utils
* Prefer business terminology over technical jargon
