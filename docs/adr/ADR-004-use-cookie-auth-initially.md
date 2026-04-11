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

## Alternatives Considered

### JWT Bearer Tokens Initially

Rejected because:

* adds unnecessary complexity for bootstrap phase
* premature for current requirements

