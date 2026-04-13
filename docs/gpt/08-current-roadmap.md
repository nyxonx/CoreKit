# Current Roadmap Snapshot

Detaljan roadmap se vodi u `docs/roadmap/README.md`.

## Current Phase

Phase 12 - Tenant Membership And Authorization Model

## Current Task

Posle zatvaranja Phase 11 fokus prelazi na tenant membership, tenant-scoped authorization i audit identitet iznad postojeceg centralnog auth modela.

## Next Tasks

- Definisati tenant membership model i vezu sa centralnim `AppUser`
- Uvesti osnovu za tenant-scoped role/permission proveru
- Dodati shared current user / current tenant / audit contract za business operacije
- Primeniti prvi authorization check na `Customers` modul
- Potvrditi dokumentacijom razliku izmedju centralnog identity store-a i tenant-scoped prava

## After That

- Sirenje business modula ili dublji security/operations milestone na stabilnijem authorization temelju
