# Current Roadmap Snapshot

Detaljan roadmap se vodi u `docs/roadmap/README.md`.

## Current Phase

Phase 18 - Tenant Registry Decoupling And Multi-AppHost Foundation

## Current Task

Uveden je prvi `TenantRegistry` baseline: tenant resolution vise ne zavisi direktno od `TenantCatalogDbContext`, vec od `ITenantRegistry` contract-a i lokalnog DB-backed adaptera. Sledeci korak je da na tom cistijem rezu definisemo remote registry API pravac i dalje host wiring granice.

## Next Tasks

- Definisati granicu izmedju centralnog identity/user ownership-a i tenant/business host potrosnje auth i membership podataka
- Definisati remote registry API podfazu iznad postojeceg `TenantRegistry` contract-a
- Isplanirati host wiring i stabilizaciju na osnovu novog registry sloja

## After That

- Krenuti u implementacione podfaze `18A-18E` na jasno definisanoj arhitektonskoj osnovi
