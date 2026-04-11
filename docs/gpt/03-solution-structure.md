# CoreKit Solution Structure

## Root Structure

```text
/src
/tests
/docs
/build
```

## Application Host Projects

```text
/src/CoreKit.AppHost.Server
/src/CoreKit.AppHost.Client
/src/CoreKit.AppHost.Contracts
```

### Server

Hosts:

* Minimal APIs
* Auth endpoints
* RPC endpoint
* Module registrations
* Middleware
* Startup orchestration

### Client

Hosts:

* Blazor WebAssembly UI
* PWA configuration
* Authentication state
* Shell/Layout

### Contracts

Shared contracts between client/server:

* DTOs
* Request/Response models
* Shared enums/constants

## Building Blocks

```text
/src/BuildingBlocks/CoreKit.BuildingBlocks.Domain
/src/BuildingBlocks/CoreKit.BuildingBlocks.Application
/src/BuildingBlocks/CoreKit.BuildingBlocks.Infrastructure
/src/BuildingBlocks/CoreKit.BuildingBlocks.Presentation
```

Purpose:

* Shared abstractions
* Base classes
* Cross-cutting utilities
* Common pipeline logic

## Modules

```text
/src/Modules/Identity
/src/Modules/Tenancy
/src/Modules/[FutureModules]
```

Each module structure:

```text
/ModuleName
    /CoreKit.Modules.[Module].Domain
    /CoreKit.Modules.[Module].Application
    /CoreKit.Modules.[Module].Infrastructure
    /CoreKit.Modules.[Module].Presentation
```

## Tests

```text
/tests/UnitTests
/tests/IntegrationTests
```

## Documentation

```text
/docs/architecture
/docs/gpt
/docs/adr
/docs/roadmap
```
