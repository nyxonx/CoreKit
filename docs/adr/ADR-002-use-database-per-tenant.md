# ADR-002: Use Database Per Tenant Multi-Tenancy Strategy

## Status

Accepted

## Context

CoreKit must support multiple tenants with strict data isolation.

Tenants may represent:

* independent companies
* independent customers
* white-label clients

Security and isolation are critical.

## Decision

Use Database Per Tenant strategy.

Each tenant receives:

* dedicated SQL database

Separate Tenant Catalog database stores:

* Tenant metadata
* Connection strings
* Tenant configuration

## Consequences

### Positive

* Strong isolation
* Easier backup/restore per tenant
* Easier scaling of large tenants
* Simplified compliance/security reasoning

### Negative

* More operational complexity
* More migration complexity
* More provisioning complexity

## Alternatives Considered

### Shared Database / Shared Schema

Rejected because:

* weak isolation
* greater risk of data leakage

### Shared Database / Separate Schema

Rejected because:

* adds complexity without full isolation
