# Current Roadmap Snapshot

Detaljan roadmap se vodi u `docs/roadmap/README.md`.

## Current Phase

Phase 18 - Tenant Registry Decoupling And Multi-AppHost Foundation

## Current Task

Uveden je `TenantRegistry` baseline i prvi remote wiring korak: tenant resolution vise ne zavisi direktno od `TenantCatalogDbContext`, postoji lokalni DB-backed adapter za platform host, `PlatformAppHost` izlaze tenant registry endpoint-e, a tenant host je konfigurisan da koristi remote registry adapter prema platform hostu.

## Next Tasks

- Definisati granicu izmedju centralnog identity/user ownership-a i tenant/business host potrosnje auth i membership podataka
- Isplanirati host wiring i stabilizaciju na osnovu novog registry sloja
- Potvrditi i dodatno ucvrstiti remote registry host wiring pre narednih ekstrakcija

## After That

- Krenuti u implementacione podfaze `18A-18E` na jasno definisanoj arhitektonskoj osnovi
