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

Business podaci ostaju `database-per-tenant`.
Identity store je trenutno centralizovan i ne prati tenant bazu jedan-na-jedan.

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

Tenant resolution pre auth toka ne znaci da je i sam identity persistence tenant-scoped.
To znaci da request dobija tenant kontekst pre user lookup i kasnijih tenant-aware authorization pravila tamo gde su obavezna.

## DbContext Rules

All tenant-specific DbContexts must:

* be tenant-aware
* use tenant connection string
* never leak data across tenants

## Security Rules

* Tenant boundary is critical security boundary
* No request may execute without resolved tenant where tenant context is required
* Tenant context must be validated before auth/user lookup

## Authorization Direction

CoreKit trenutno razdvaja:

* centralizovanu autentikaciju (`AppUser`, cookie auth, jedan identity store)
* tenant-scoped membership i autorizaciju za business module

Planirani pravac nije `user database per tenant`, vec:

* jedan globalni `UserId`
* membership zapis po tenant-u
* tenant-scoped role ili permission model
* audit identitet koji nosi i `UserId` i `TenantId`

## Current Phase 12 Baseline

Trenutni uvodni `Phase 12` baseline vec uvodi:

* `AppUserTenantMembership` u centralnom Identity store-u
* shared current execution context contract za application/use case sloj
* shared proveru da li autentikovani korisnik pripada aktivnom tenant-u
* prvi primenjeni authorization check na `Customers` RPC operacijama
* pocetni audit metadata obrazac u `Customers` za `CreatedByUserId`, `ModifiedByUserId`, tenant identitet i UTC timestamp polja
