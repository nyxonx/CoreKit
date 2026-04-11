# CoreKit Authentication and Tenancy

## Authentication Strategy

Initial implementation uses:

* ASP.NET Core Identity
* Cookie Authentication

## Authentication Features

Required:

* Login
* Logout
* Auth State Persistence
* Protected Routes
* Role Support
* Permission-Ready Design

## Identity Models

Core models:

* AppUser
* AppRole

## Multi-Tenancy Strategy

Database Per Tenant

## Tenant Catalog Database

Stores:

* TenantId
* Name
* ConnectionString
* Host/Subdomain
* Status
* Metadata

## Tenant Resolution

Supported strategies:

1. Subdomain
2. Header
3. Hostname

Resolution order may evolve.

## Tenant Lifecycle

Tenant must be resolved:

1. Before Authentication
2. Before DbContext creation
3. Before Request Handling

## DbContext Rules

All tenant-specific DbContexts must:

* be tenant-aware
* use tenant connection string
* never leak data across tenants

## Security Rules

* Tenant boundary is critical security boundary
* No request may execute without resolved tenant where tenant context is required
* Tenant context must be validated before auth/user lookup
