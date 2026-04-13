# CoreKit

CoreKit je reusable enterprise-grade application core/framework za izgradnju modularnih poslovnih aplikacija na zajednickoj platformi, arhitekturi i operativnim obrascima.

Primeri ciljnih domena:
- ERP
- CRM
- POS
- Hotel Management
- Restaurant Management
- Booking Systems
- Inventory / Warehouse
- Billing / Invoicing

## Goals

- maksimalna ponovna upotreba koda izmedju projekata
- brz razvoj novih business aplikacija
- visoka odrzivost i jasna arhitektura
- modularno sirenje kroz nove funkcionalnosti
- production-grade kvalitet bez nagomilavanja tehnickog duga

## Architecture Direction

CoreKit je postavljen kao:
- modular monolith
- domain-driven i CQRS-oriented platforma na .NET 10
- Blazor WebAssembly Hosted + Minimal APIs + PWA
- baza za multi-tenant i white-label resenja

Kljucne odluke su dokumentovane u [`docs/adr`](C:/Users/nikol/source/repos/nyxonx/CoreKit/docs/adr).

## Repository Structure

```text
/src
/tests
/docs
/.github
/build
```

- `src/` sadrzi produkcione projekte
- `tests/` sadrzi test projekte
- `docs/` sadrzi arhitekturu, ADR-ove, roadmap i AI kontekst
- `.github/` sadrzi repo smernice
- `build/` sadrzi build/deployment skripte i pomocne automatizacije

## Current Status

Osnovna platforma i roadmap faze `0-10` su zavrsene.

Trenutno stanje repozitorijuma ukljucuje:
- `Identity` modul za ASP.NET Core Identity i cookie auth tok
- `Tenancy` modul za tenant catalog, tenant resolution i database-per-tenant osnovu
- `Customers` modul kao prvi end-to-end poslovni modul kroz application, infrastructure, RPC i UI sloj

Platforma danas vec pokriva:
- modularni startup kroz module catalog i shared contracts
- unified `POST /api/rpc` endpoint za business CQRS operacije
- tenant-aware persistence i tenant provisioning osnovu
- production-readiness baseline za logging, health, caching i background jobs

Detaljan plan razvoja vodi se u [`docs/roadmap/README.md`](C:/Users/nikol/source/repos/nyxonx/CoreKit/docs/roadmap/README.md).
