# Current Roadmap Snapshot

Detaljan roadmap se vodi u `docs/roadmap/README.md`.

## Current Phase

Phase 16 - Completed

## Current Task

`PlatformAppHost` cleanup je zavrsen: platform client i platform server su procisceni od tranzicionih tenant/control-plane workaround ostataka, platform auth koristi uzi DTO, `Customers` vise nije registrovan u platform hostu, a lokalne SQLite baze su prebacene u zajednicki `localdata/` folder na nivou repozitorijuma.

## Next Tasks

- Odluciti da li slede tenant AppHost cleanup ili novi platform feature slice
- Po potrebi izdvojiti shared server infrastructure (`Diagnostics` / `Rpc`) kao zaseban refactor
- Nastaviti samo na cistijoj dual-AppHost osnovi bez vracanja platform logike u tenant host

## After That

- Otvoriti sledecu fazu sa jasno suzenim scope-om: tenant cleanup ili novi platform feature work
