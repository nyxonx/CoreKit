# CoreKit

CoreKit je reusable enterprise-grade application core/framework namenjen za ubrzani razvoj buducih poslovnih aplikacija kroz zajednicku arhitekturu, infrastrukturu i module.

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
- domain-driven i CQRS-oriented platforma
- baza za multi-tenant i white-label resenja

Ključne odluke su dokumentovane u [`docs/adr`](C:/Users/nikol/source/repos/nyxonx/CoreKit/docs/adr).

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

Bootstrap faza je zavrsena.

Sledeci korak:
- kreiranje solution skeleton-a i prvih projekata unutar `src/`

Detaljan plan razvoja vodi se u [`docs/roadmap/README.md`](C:/Users/nikol/source/repos/nyxonx/CoreKit/docs/roadmap/README.md).
