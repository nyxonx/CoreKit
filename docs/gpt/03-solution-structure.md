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
/src/CoreKit.PlatformAppHost.Server
/src/CoreKit.PlatformAppHost.Client
/src/CoreKit.AppHost.Contracts
```

### Tenant AppHost

Hosts:

* Minimal APIs
* Auth endpoints
* RPC endpoint
* Module registrations
* Middleware
* Startup orchestration
* Tenant-scoped UI hosting
* Active tenant routing and tenant-scoped admin surface

Current concrete projects:

* `CoreKit.AppHost.Server`
* `CoreKit.AppHost.Client`

### Platform AppHost

Hosts:

* Control-plane Minimal APIs
* Platform auth endpoints
* RPC endpoint over shared modules/contracts
* Platform admin UI hosting
* Dedicated platform layout/navigation
* Global admin control-plane operations

Current concrete projects:

* `CoreKit.PlatformAppHost.Server`
* `CoreKit.PlatformAppHost.Client`

### Contracts

Shared contracts between client/server:

* DTOs
* Request/Response models
* Shared enums/constants

These contracts are shared across both AppHost pairs.

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
* Shared module registration contracts

Current note:

* `CoreKit.BuildingBlocks.Domain` i `CoreKit.BuildingBlocks.Infrastructure` mogu privremeno sadrzati samo marker ili mali broj tipova
* ne uvoditi shared domain/infrastructure baze unapred bez stvarne ponovne upotrebe kroz vise modula

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

Recommended module contents:

* `Domain`: business state and rules
* `Application`: commands, queries, handlers, DTOs
* `Infrastructure`: persistence and external integrations
* `Presentation`: endpoint mapping and module entry point

Recommended host integration:

* module class implementing the shared module contract
* module added to the relevant AppHost module catalog
* optional client contracts in `CoreKit.AppHost.Contracts`
* optional WASM module client in `CoreKit.AppHost.Client` and/or `CoreKit.PlatformAppHost.Client`, depending on where the UI surface lives

## Tests

```text
/tests/CoreKit.Modules.[Module].Tests
/tests/TestInfrastructure
```

Current approach:

* test projekti su trenutno organizovani primarno po modulu, ne po globalnoj `UnitTests` / `IntegrationTests` podeli
* jedan modulski test projekat moze da sadrzi i unit i integration scenarije dok god je scope jasan
* `TestInfrastructure` sadrzi shared test support tipove koji se koriste iz vise test projekata

## Documentation

```text
/docs/architecture
/docs/gpt
/docs/adr
/docs/roadmap
```
