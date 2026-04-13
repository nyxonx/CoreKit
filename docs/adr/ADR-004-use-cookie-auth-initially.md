# ADR-004: Use Cookie Authentication for Initial Version

## Status

Accepted

## Context

Initial CoreKit version targets:

* Blazor WebAssembly Hosted
* PWA
* Web-first usage

Primary need:

* simple secure authentication
* minimal auth complexity during bootstrap

## Decision

Use ASP.NET Core Identity with Cookie Authentication for initial implementation.

For the current platform baseline, Identity remains centralized in a dedicated store.
Tenant-specific business data and later tenant-scoped authorization evolve separately on top of that auth model.

## Consequences

### Positive

* Simpler setup
* Mature and proven approach
* Good integration with ASP.NET Core Identity
* Suitable for hosted Blazor applications

### Negative

* Less ideal for external/mobile API consumers
* Harder future federation/API scenarios

## Future Consideration

May evolve to:

* JWT / Access Token auth
* Hybrid cookie/token approach
* Richer tenant membership and authorization model above centralized Identity

## Alternatives Considered

### JWT Bearer Tokens Initially

Rejected because:

* adds unnecessary complexity for bootstrap phase
* premature for current requirements
