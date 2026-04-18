# Current Roadmap Snapshot

Detaljan roadmap se vodi u `docs/roadmap/README.md`.

## Current Phase

Phase 18 - Tenant Registry Decoupling And Multi-AppHost Foundation

## Current Task

Definisati ciljnu arhitekturu u kojoj je `PlatformAppHost` jedini owner tenant kataloga, dok tenant/business AppHost-ovi tenant informacije dobijaju kroz `TenantRegistry` sloj, uz jasno planiran i dokumentovan put i za buduci remote API model.

## Next Tasks

- Definisati odgovornosti `PlatformAppHost` i tenant/business AppHost-ova
- Uvesti `TenantRegistry` contract i read modele koji odvajaju hostove od direktnog pristupa catalog bazi
- Definisati granicu izmedju centralnog identity/user ownership-a i tenant/business host potrosnje auth i membership podataka
- Isplanirati lokalni adapter, remote API podfazu i stabilizaciju kao deo iste zaokruzene arhitektonske faze

## After That

- Krenuti u implementacione podfaze `18A-18E` na jasno definisanoj arhitektonskoj osnovi
