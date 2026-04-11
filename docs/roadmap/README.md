# CoreKit Development Roadmap

Ovaj dokument je radna tabla za razvoj projekta `CoreKit`.

Kada otvoris roadmap, treba odmah da bude jasno:
- sta je zavrseno
- sta je trenutno aktivno
- sta ide sledece

## Status Legend

- `[x]` Zavrseno
- `[>]` Trenutno radimo
- `[ ]` Sledece / planirano
- `[~]` Zapoceto, ali nije zavrseno

## How To Use

- Jedan task treba da stane u blok od `30-60 min`
- Kada task zavrsis, prebaci ga iz `[ ]` u `[x]`
- Samo jedan task oznaci kao `[>]`
- Ako je nesto krenuto ali nije kompletno, koristi `[~]`
- Ako se promeni pravac, azuriraj i fazu i sledece korake

---

## Phase 0 - Project Bootstrap

Goal:
Postaviti repozitorijum, osnovnu strukturu i pocetnu dokumentaciju.

Status: Completed

Tasks:

- `[x]` Kreirati osnovnu folder strukturu (`src`, `tests`, `docs`, `.github`)
- `[x]` Dodati `.gitignore` za Visual Studio i .NET
- `[x]` Dodati osnovne GPT/context dokumente
- `[x]` Dodati pocetne ADR dokumente
- `[x]` Dodati solution foldere da dokumentacija bude vidljiva u Visual Studio
- `[x]` Azurirati glavni `README.md` sa opisom projekta i strukture
- `[x]` Dodati `build/` folder i objasniti njegovu namenu

Exit criteria:
- Repo ima jasnu strukturu
- Osnovna dokumentacija postoji
- Razvoj moze da krene bez dodatnog reorganizovanja

---

## Phase 1 - Solution Skeleton

Goal:
Napraviti stvarne projekte i reference prema ciljnoj arhitekturi.

Status: Completed

Tasks:

- `[x]` Kreirati `CoreKit.AppHost.Server`
- `[x]` Kreirati `CoreKit.AppHost.Client`
- `[x]` Kreirati `CoreKit.AppHost.Contracts`
- `[x]` Dodati projekte u solution
- `[x]` Podesiti project references izmedju AppHost projekata
- `[x]` Kreirati `BuildingBlocks` foldere i prazne projekte
- `[x]` Kreirati `Modules/Identity` strukturu sa 4 projekta
- `[x]` Kreirati `Modules/Tenancy` strukturu sa 4 projekta
- `[x]` Proveriti da solution build prolazi bez poslovne logike

Exit criteria:
- Svi glavni projekti postoje
- Reference su validne
- Solution se uspesno build-uje

---

## Phase 2 - App Host And Vertical Slice Bootstrap

Goal:
Podesiti server, klijent i osnovni startup tok bez pune poslovne logike.

Status: Completed

Tasks:

- `[x]` Podesiti Minimal API server startup
- `[x]` Odrzati `Program.cs` cistim kroz extension metode
- `[x]` Podesiti osnovni Blazor WASM host
- `[x]` Ukljuciti PWA konfiguraciju
- `[x]` Dodati osnovni layout i shell stranicu
- `[x]` Dodati health endpoint
- `[x]` Dodati osnovni config loading za app settings
- `[x]` Definisati startup registraciju modula

Exit criteria:
- Server i client se pokrecu
- Postoji osnovni UI shell
- Infrastrukturni endpointi rade

---

## Phase 3 - Identity And Cookie Authentication

Goal:
Napraviti prvi kompletan login/logout tok prema ADR-004.

Status: In progress

Tasks:

- `[>]` Uvesti ASP.NET Core Identity u server
- `[ ]` Definisati `AppUser`
- `[ ]` Definisati `AppRole`
- `[ ]` Podesiti cookie authentication
- `[ ]` Dodati login endpoint
- `[ ]` Dodati logout endpoint
- `[ ]` Dodati endpoint za proveru auth state-a
- `[ ]` Napraviti login stranicu u client-u
- `[ ]` Dodati auth state provider ili odgovarajuci klijentski mehanizam
- `[ ]` Obezbediti protected routes
- `[ ]` Dodati seed za admin korisnika i osnovnu rolu

Exit criteria:
- Korisnik moze da se prijavi i odjavi
- Zasticene rute rade
- Auth state prezivljava refresh

---

## Phase 4 - Tenant Catalog And Tenant Resolution

Goal:
Postaviti multi-tenant osnovu prema ADR-002 pre tenant-aware poslovne logike.

Status: Planned

Tasks:

- `[ ]` Definisati model tenant kataloga
- `[ ]` Kreirati catalog DbContext
- `[ ]` Dodati migraciju za tenant katalog
- `[ ]` Implementirati tenant resolution po host/header strategiji
- `[ ]` Uvesti `TenantContext`
- `[ ]` Obezbediti da se tenant razresi pre autentikacije
- `[ ]` Obezbediti da se tenant razresi pre DbContext kreiranja
- `[ ]` Dodati validaciju za nepostojeci ili neaktivan tenant
- `[ ]` Dodati test scenarije za tenant resolution

Exit criteria:
- Tenant moze pouzdano da se razresi po request-u
- Nema obrade request-a bez tenant konteksta tamo gde je potreban

---

## Phase 5 - Tenant Data Access

Goal:
Podesiti tenant-aware persistence i otkloniti rizik mesanja podataka.

Status: Planned

Tasks:

- `[ ]` Dizajnirati tenant connection string provider
- `[ ]` Napraviti tenant DbContext factory
- `[ ]` Podesiti tenant-specific DbContext registraciju
- `[ ]` Obezbediti izolaciju konekcija po tenant-u
- `[ ]` Dodati startup proveru tenant konfiguracije
- `[ ]` Dodati prvi integration test za izolaciju tenant podataka
- `[ ]` Dokumentovati pravila za tenant-aware persistence

Exit criteria:
- Svaki tenant koristi svoju bazu
- Ne postoji curenje podataka izmedju tenant-a

---

## Phase 6 - CQRS And Unified RPC Pipeline

Goal:
Uvesti standardizovan execution pipeline za business operacije prema ADR-003.

Status: Planned

Tasks:

- `[ ]` Dodati MediatR
- `[ ]` Definisati bazne `Command` i `Query` apstrakcije
- `[ ]` Definisati result model za handlere
- `[ ]` Dodati validation pipeline behavior
- `[ ]` Dodati logging pipeline behavior
- `[ ]` Napraviti `POST /api/rpc` endpoint
- `[ ]` Implementirati request dispatching
- `[ ]` Dodati prvi end-to-end sample command
- `[ ]` Dodati prvi end-to-end sample query

Exit criteria:
- Business operacije prolaze kroz jedinstven pipeline
- Client moze da pozove sample command i query

---

## Phase 7 - Module Framework

Goal:
Napraviti ponovljiv obrazac za dodavanje buducih modula.

Status: Planned

Tasks:

- `[ ]` Definisati module registration contract
- `[ ]` Definisati module startup pattern
- `[ ]` Definisati module service registration pattern
- `[ ]` Dodati base contracts u `BuildingBlocks`
- `[ ]` Dodati primer wiring-a za `Identity` modul
- `[ ]` Dodati primer wiring-a za `Tenancy` modul
- `[ ]` Napraviti template/checklist za novi modul
- `[ ]` Dokumentovati pravila granica izmedju modula

Exit criteria:
- Novi modul moze da se doda bez improvizacije
- Granice modula su jasne i ponovljive

---

## Phase 8 - First Real Module

Goal:
Proveriti da arhitektura radi na jednoj pravoj poslovnoj funkcionalnosti.

Status: Planned

Tasks:

- `[ ]` Izabrati prvi pilot modul
- `[ ]` Definisati use case-eve modula
- `[ ]` Dodati domain modele
- `[ ]` Dodati command/query handlere
- `[ ]` Dodati persistence mapiranje
- `[ ]` Dodati RPC operacije za modul
- `[ ]` Dodati osnovni UI ekran
- `[ ]` Dodati unit testove
- `[ ]` Dodati integration testove

Exit criteria:
- Jedan modul radi end-to-end kroz ceo stack
- Arhitektura je proverena na realnom primeru

---

## Phase 9 - Migrations And Provisioning

Goal:
Automatizovati podizanje kataloga, tenant baza i upgrade tok.

Status: Planned

Tasks:

- `[ ]` Dodati catalog migration runner
- `[ ]` Dodati tenant migration runner
- `[ ]` Definisati tenant provisioning flow
- `[ ]` Dodati inicijalno kreiranje tenant baze
- `[ ]` Dodati seed osnovnih podataka po tenant-u
- `[ ]` Dodati startup migration executor gde ima smisla
- `[ ]` Dodati osnovne deployment skripte

Exit criteria:
- Novi tenant moze da se podigne kontrolisano
- Migracije su standardizovane

---

## Phase 10 - Production Readiness

Goal:
Podici platformu na nivo spreman za stvaran rad.

Status: Planned

Tasks:

- `[ ]` Standardizovati error handling
- `[ ]` Uvesti strukturisano logovanje
- `[ ]` Dodati audit logging osnovu
- `[ ]` Prosiriti health checks
- `[ ]` Uvesti osnovnu observability strategiju
- `[ ]` Definisati caching strategiju
- `[ ]` Definisati background jobs pristup
- `[ ]` Dodati osnovne security hardening provere

Exit criteria:
- Platforma ima operativne osnove za produkciju
- Kriticne tehnicke rupe su zatvorene

---

## Current Focus

Now:
- `Phase 0` je zavrsena
- `Phase 1` je zavrsena
- `Phase 2` je zavrsena
- Aktivna je `Phase 3`
- Sledece uvodimo Identity i cookie auth

After that:
- Posle toga tenancy
- Zatim RPC i CQRS
