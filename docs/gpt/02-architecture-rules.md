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

## Naming Rules

* Clear, explicit, enterprise-oriented naming
* Avoid vague names like Manager, Helper, Utils
* Prefer business terminology over technical jargon
