# Current Roadmap Snapshot

Detaljan roadmap se vodi u `docs/roadmap/README.md`.

## Current Phase

Phase 14 - Platform AppHost Extraction And Control Plane Foundation

## Current Task

Phase 14 ekstrakcija je prakticno izvedena: control-plane je preseljen u poseban `CoreKit.PlatformAppHost.Server` + `CoreKit.PlatformAppHost.Client`, dok je tenant AppHost vracen na tenant-only odgovornost. Otvoren je jos zavrsni stabilization korak oko test scenarija i dual-host smoke provere kada lokalni test run bude stabilan.

## Next Tasks

- Ostaviti `Phase 14` na zavrsnom stabilization pass-u dok ne prodjemo pouzdan test run i dual-host smoke proveru
- Zadrzati dual-AppHost docs alignment kao baseline za narednu fazu
- Ne siriti nove platform feature-e dok se ekstrakcija ne zatvori i stanje ne bude mirno

## After That

- Nastaviti platform feature-e tek na stabilnoj dual-AppHost osnovi, bez daljeg mesanja tenant i control-plane UX-a
