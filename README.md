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

Kljucne odluke su dokumentovane u [`docs/adr`](docs/adr/README.md).

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

Osnovna platforma i roadmap faze `0-12` su zavrsene. `Phase 13` je uvela razdvajanje tenant admin i platform admin surface-a, a `Phase 14` je zapocela izdvajanje platform control-plane dela u poseban AppHost.

Trenutno stanje repozitorijuma ukljucuje:
- `Identity` modul za ASP.NET Core Identity i cookie auth tok
- `Tenancy` modul za tenant catalog, tenant resolution i database-per-tenant osnovu
- `Customers` modul kao prvi end-to-end poslovni modul kroz application, infrastructure, RPC i UI sloj

Repo sada koristi dual-AppHost model:
- `CoreKit.AppHost.Server` + `CoreKit.AppHost.Client` trenutno predstavljaju tenant-facing AppHost
- `CoreKit.PlatformAppHost.Server` + `CoreKit.PlatformAppHost.Client` predstavljaju control-plane Platform AppHost
- `CoreKit.AppHost.Contracts`, `BuildingBlocks` i `Modules/*` ostaju shared osnova izmedju oba hosta

Prakticno to znaci:
- tenant host sluzi tenant-scoped korisnickom iskustvu i radu nad aktivnim tenant kontekstom
- platform host sluzi global admin control-plane operacijama kao sto su tenant catalog, provisioning i platform-level membership administracija
- `admin.local` treba da gadja `CoreKit.PlatformAppHost.Server`, dok tenant hostovi ostaju na tenant AppHost strani

Platforma danas vec pokriva:
- modularni startup kroz module catalog i shared contracts
- unified `POST /api/rpc` endpoint za business CQRS operacije
- tenant-aware persistence i tenant provisioning osnovu
- tenant administration UI za membership i role management unutar aktivnog tenant konteksta
- control-plane tenant catalog, tenant lifecycle i platform membership flow na izdvojenom platform hostu
- production-readiness baseline za logging, health, caching i background jobs

Auth i tenancy model su trenutno postavljeni tako da:
- `Identity` ostaje centralizovan kroz ASP.NET Core Identity i cookie auth
- business podaci ostaju `database-per-tenant`
- tenant-scoped membership i authorization su uvedeni iznad centralnog identity store-a
- tenant provisioning i tenant catalog administracija su izdvojeni na control-plane Platform AppHost surface

Za arhitektonske detalje oko module framework-a, startup orkestracije i dual-AppHost podele pogledati [`docs/architecture`](docs/architecture/README.md).

Detaljan plan razvoja vodi se u [`docs/roadmap/README.md`](docs/roadmap/README.md).
