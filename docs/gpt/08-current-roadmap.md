# Current Roadmap Snapshot

Detaljan roadmap se vodi u `docs/roadmap/README.md`.

## Current Phase

Phase 17 - Completed

## Current Task

Lokalna SQLite konfiguracija je prociscena: `Tenancy:DatabaseRoot` je jedini izvor za folder baza, connection string kljucevi cuvaju samo imena fajlova, seed tenant-i su uklonjeni, a clean startup sada krece bez tenant-a dok se prvi tenant kreira kroz `platform-admin`.

## Next Tasks

- Definisati ciljnu arhitekturu za vise zasebnih AppHost parova uz centralni platform/catalog owner
- Razdvojiti sta tenant-facing hostovi moraju znati direktno, a sta treba ici kroz buduci tenant registry sloj
- Otvoriti sledecu fazu sa jasno ogranicenim scope-om za tu arhitektonsku ekstrakciju

## After That

- Krenuti u sledecu arhitektonsku fazu na cistijem startup i local data modelu
