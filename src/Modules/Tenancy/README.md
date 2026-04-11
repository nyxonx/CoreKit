# Tenancy Module

Ovaj modul trenutno pokriva:
- tenant katalog
- tenant resolution
- tenant-aware persistence osnovu

## Tenant-Aware Persistence Rules

- Tenant katalog (`TenantCatalogDbContext`) i tenant data baze su odvojene.
- Tenant data pristup ide preko `ITenantConnectionStringProvider`.
- Tenant-specific `DbContext` se kreira iz `TenantContext`-a, nikad iz globalnog app config-a.
- Application flow za tenant podatke treba da koristi tenant-aware servise, ne da presentation sloj direktno zna connection string.
- Svaki tenant mora da ima sopstvenu bazu; deljenje tenant poslovnih podataka kroz isti `DbContext` nije dozvoljeno.
- Startup validacija mora da otkrije tenant bez connection string-a pre runtime rada.

## Current Runtime Pieces

- `TenantResolutionMiddleware` razresava tenant pre auth i pre tenant-specific persistence pristupa.
- `TenantConnectionStringProvider` cita connection string iz aktivnog `TenantContext`-a.
- `TenantDbContextFactory` pravi `TenantAppDbContext` za aktivni tenant.
- `TenantNoteService` je prvi primer tenant-aware application flow-a.
