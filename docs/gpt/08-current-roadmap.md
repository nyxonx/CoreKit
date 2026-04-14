# Current Roadmap Snapshot

Detaljan roadmap se vodi u `docs/roadmap/README.md`.

## Current Phase

Phase 14 - Completed

## Current Task

Phase 14 je zatvorena: control-plane je preseljen u poseban `CoreKit.PlatformAppHost.Server` + `CoreKit.PlatformAppHost.Client`, tenant AppHost je vracen na tenant-only odgovornost, build prolazi, a rucni dual-host smoke check je potvrdio trazeno razdvajanje.

## Next Tasks

- Definisati i otvoriti sledecu fazu za platform feature-e na cistoj dual-AppHost osnovi
- Zadrzati dual-AppHost docs alignment kao baseline za narednu fazu
- Ne vracati tenant i platform UX u isti host

## After That

- Nastaviti platform feature-e tek na stabilnoj dual-AppHost osnovi, bez daljeg mesanja tenant i control-plane UX-a
