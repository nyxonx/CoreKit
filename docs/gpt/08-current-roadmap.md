# Current Roadmap Snapshot

Detaljan roadmap se vodi u `docs/roadmap/README.md`.

## Current Phase

Phase 4 - Tenant Catalog And Tenant Resolution

## Current Task

Pokrenuti tenant catalog bazu, resolution middleware i `TenantContext` kroz `CoreKit.AppHost.Server`.

## Next Tasks

- Verifikovati resolution tok za `localhost` i `X-Tenant` header
- Dodati EF migraciju za tenant katalog
- Uvesti prve test scenarije za tenant resolution

## After That

- Unified RPC + CQRS pipeline
- Module framework + prvi pravi modul
