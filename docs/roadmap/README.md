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

Status: Completed

Tasks:

- `[x]` Uvesti ASP.NET Core Identity u server
- `[x]` Definisati `AppUser`
- `[x]` Definisati `AppRole`
- `[x]` Podesiti cookie authentication
- `[x]` Dodati login endpoint
- `[x]` Dodati logout endpoint
- `[x]` Dodati endpoint za proveru auth state-a
- `[x]` Napraviti login stranicu u client-u
- `[x]` Dodati auth state provider ili odgovarajuci klijentski mehanizam
- `[x]` Obezbediti protected routes
- `[x]` Dodati seed za admin korisnika i osnovnu rolu

Exit criteria:
- Korisnik moze da se prijavi i odjavi
- Zasticene rute rade
- Auth state prezivljava refresh

Current focus:
- `[x]` Verifikovati hosted login/logout tok kroz `AppHost.Server`
- `[x]` Potvrditi da auth state prezivljava refresh u browser-u
- `[x]` Dodati zavrsni auth UX polish posle prve provere

---

## Phase 4 - Tenant Catalog And Tenant Resolution

Goal:
Postaviti multi-tenant osnovu prema ADR-002 pre tenant-aware poslovne logike.

Status: Completed

Tasks:

- `[x]` Definisati model tenant kataloga
- `[x]` Kreirati catalog DbContext
- `[x]` Dodati migraciju za tenant katalog
- `[x]` Implementirati tenant resolution po host/header strategiji
- `[x]` Uvesti `TenantContext`
- `[x]` Obezbediti da se tenant razresi pre autentikacije
- `[x]` Obezbediti da se tenant razresi pre DbContext kreiranja
- `[x]` Dodati validaciju za nepostojeci ili neaktivan tenant
- `[x]` Dodati test scenarije za tenant resolution

Exit criteria:
- Tenant moze pouzdano da se razresi po request-u
- Nema obrade request-a bez tenant konteksta tamo gde je potreban

---

## Phase 5 - Tenant Data Access

Goal:
Podesiti tenant-aware persistence i otkloniti rizik mesanja podataka.

Status: Completed

Tasks:

- `[x]` Dizajnirati tenant connection string provider
- `[x]` Napraviti tenant DbContext factory
- `[x]` Podesiti tenant-specific DbContext registraciju
- `[x]` Obezbediti izolaciju konekcija po tenant-u
- `[x]` Dodati startup proveru tenant konfiguracije
- `[x]` Dodati prvi integration test za izolaciju tenant podataka
- `[x]` Dokumentovati pravila za tenant-aware persistence

Exit criteria:
- Svaki tenant koristi svoju bazu
- Ne postoji curenje podataka izmedju tenant-a

---

## Phase 6 - CQRS And Unified RPC Pipeline

Goal:
Uvesti standardizovan execution pipeline za business operacije prema ADR-003.

Status: Completed

Tasks:

- `[x]` Dodati MediatR
- `[x]` Definisati bazne `Command` i `Query` apstrakcije
- `[x]` Definisati result model za handlere
- `[x]` Dodati validation pipeline behavior
- `[x]` Dodati logging pipeline behavior
- `[x]` Napraviti `POST /api/rpc` endpoint
- `[x]` Implementirati request dispatching
- `[x]` Dodati prvi end-to-end sample command
- `[x]` Dodati prvi end-to-end sample query

Exit criteria:
- Business operacije prolaze kroz jedinstven pipeline
- Client moze da pozove sample command i query

---

## Phase 7 - Module Framework

Goal:
Napraviti ponovljiv obrazac za dodavanje buducih modula.

Status: Completed

Tasks:

- `[x]` Definisati module registration contract
- `[x]` Definisati module startup pattern
- `[x]` Definisati module service registration pattern
- `[x]` Dodati base contracts u `BuildingBlocks`
- `[x]` Dodati primer wiring-a za `Identity` modul
- `[x]` Dodati primer wiring-a za `Tenancy` modul
- `[x]` Napraviti template/checklist za novi modul
- `[x]` Dokumentovati pravila granica izmedju modula

Reference docs:
- `docs/architecture/module-framework.md`
- `docs/architecture/new-module-checklist.md`

Exit criteria:
- Novi modul moze da se doda bez improvizacije
- Granice modula su jasne i ponovljive

---

## Phase 8 - First Real Module

Goal:
Proveriti da arhitektura radi na jednoj pravoj poslovnoj funkcionalnosti.

Status: Completed

Tasks:

- `[x]` Izabrati prvi pilot modul
- `[x]` Definisati use case-eve modula
- `[x]` Dodati domain modele
- `[x]` Dodati command/query handlere
- `[x]` Dodati persistence mapiranje
- `[x]` Dodati RPC operacije za modul
- `[x]` Dodati osnovni UI ekran
- `[x]` Dodati unit testove
- `[x]` Dodati integration testove

Exit criteria:
- Jedan modul radi end-to-end kroz ceo stack
- Arhitektura je proverena na realnom primeru

---

## Phase 9 - Migrations And Provisioning

Goal:
Automatizovati podizanje kataloga, tenant baza i upgrade tok.

Status: Completed

Tasks:

- `[x]` Dodati catalog migration runner
- `[x]` Dodati tenant migration runner
- `[x]` Definisati tenant provisioning flow
- `[x]` Dodati inicijalno kreiranje tenant baze
- `[x]` Dodati seed osnovnih podataka po tenant-u
- `[x]` Dodati startup migration executor gde ima smisla
- `[x]` Dodati osnovne deployment skripte

Exit criteria:
- Novi tenant moze da se podigne kontrolisano
- Migracije su standardizovane

---

## Phase 10 - Production Readiness

Goal:
Podici platformu na nivo spreman za stvaran rad.

Status: Completed

Tasks:

- `[x]` Standardizovati error handling
- `[x]` Uvesti strukturisano logovanje
- `[x]` Dodati audit logging osnovu
- `[x]` Prosiriti health checks
- `[x]` Uvesti osnovnu observability strategiju
- `[x]` Definisati caching strategiju
- `[x]` Definisati background jobs pristup
- `[x]` Dodati osnovne security hardening provere

Exit criteria:
- Platforma ima operativne osnove za produkciju
- Kriticne tehnicke rupe su zatvorene

---

## Phase 11 - Architecture Hardening And Cleanup

Goal:
Ucvrstiti platformsku arhitekturu, smanjiti bootstrap tehnicki dug i uskladiti dokumentaciju sa stvarnim stanjem sistema pre daljeg sirenja modula.

Status: Completed

Tasks:

- `[x]` Refaktorisati module startup orchestration kroz unified `InitializeAsync` pipeline
- `[x]` Ukloniti direktno module-specific init pozivanje iz `Program.cs`
- `[x]` Standardizovati C# formatting kroz ceo solution
- `[x]` Dodati `.editorconfig` i format guardrail-e
- `[x]` Uskladiti root `README.md` i ostalu high-level dokumentaciju sa realnim stanjem repozitorijuma
- `[x]` Dodati integration test koji verifikuje tenant-before-auth redosled
- `[x]` Auditirati test coverage za prvi business modul i evidentirati rupe
- `[x]` Dokumentovati startup orchestration flow za module

Exit criteria:

- `Program.cs` ostaje cist i ne zna detalje pojedinacnih modula
- Module initialization ide kroz standardizovan shared pipeline
- Dokumentacija odgovara stvarnom stanju koda
- Formatting i style pravila su standardizovani i ponovljivi
- Postoji test ili jasna verifikacija da tenant resolution ide pre auth toka tamo gde je to obavezno

---

## Phase 12 - Tenant Membership And Authorization Model

Goal:
Uvesti tenant-scoped membership, autorizaciju i audit identitet iznad postojeceg centralizovanog auth modela, bez menjanja osnovnog `ASP.NET Core Identity + cookie auth` pravca.

Status: Completed

Tasks:

- `[x]` Definisati tenant membership model koji povezuje centralni `AppUser` sa aktivnim tenant-ima
- `[x]` Definisati tenant-scoped role/permission osnovu za buduce module
- `[x]` Dodati admin-only server tok za tenant membership dodelu i promenu role
- `[x]` Dodati shared current user / current tenant access contract za business operacije
- `[x]` Uvesti proveru da autentikovani korisnik pripada aktivnom tenant-u tamo gde je to obavezno
- `[x]` Definisati audit metadata obrazac (`CreatedByUserId`, `ModifiedByUserId`, `TenantId`)
- `[x]` Primeniti osnovnu tenant authorization proveru na `Customers` modul
- `[x]` Dodati test scenarije za tenant membership i authorization flow
- `[x]` Uskladiti auth/tenancy dokumentaciju sa central identity + tenant-scoped authorization modelom

Exit criteria:

- Autentikacija ostaje centralizovana, ali je pristup podacima i operacijama tenant-scoped
- Tenant membership i osnovna autorizacija su jasno modelovani i proverljivi
- Business moduli imaju standardan obrazac za current user, current tenant i audit identitet
- Dokumentacija jasno razlikuje centralni identity store od tenant-scoped prava

---

## Phase 13 - Tenant Administration UI And Management Flows

Goal:
Uvesti prvi stvarni admin UI za tenant administraciju, kreiranje tenant-a i upravljanje korisnicima po tenant-u iznad vec zavrsenog server-side membership i authorization modela.

Status: In Progress

Tasks:

- `[x]` Dodati prvi tenant administration ekran za aktivni tenant
- `[x]` Povezati membership list/upsert server tokove na Blazor client kroz module client obrazac
- `[x]` Dodati UI za promenu tenant role po korisniku bez ručnog RPC testiranja
- `[x]` Dodati UI za kreiranje novog tenant-a i provisioning flow
- `[x]` Dodati pregled aktivnog tenant konteksta u admin povrsini
- `[x]` Dodati osnovni guardrail UX za non-admin korisnike na admin ekranima
- `[ ]` Dodati test scenarije za tenant administration UI tokove tamo gde je prakticno
- `[ ]` Uskladiti README i high-level dokumentaciju sa tenant administration UI baseline-om

Exit criteria:

- Postoji tenant administration UI koji koristi postojece server-side tokove
- Tenant admin moze da vidi i menja membership/role za aktivni tenant
- Tenant creation/provisioning ima jasan UI ili proverljiv flow
- Admin povrsina jasno razlikuje tenant admin i non-admin iskustvo

---

## Current Focus

Now:
- `Phase 0` je zavrsena
- `Phase 1` je zavrsena
- `Phase 2` je zavrsena
- `Phase 3` je zavrsena
- `Phase 4` je zavrsena
- `Phase 5` je zavrsena
- `Phase 6` je zavrsena
- `Phase 7` je zavrsena
- `Phase 8` je zavrsena
- `Customers` modul radi end-to-end kroz tenant-aware persistence, RPC i UI
- `Phase 9` je zavrsena
- Catalog migracije, tenant provisioning i deployment helper tok su standardizovani
- `Phase 10` je zavrsena
- Uvedeni su global exception handling, request context logging i prosireni health checks
- Dodati su audit dogadjaji za auth i RPC, runtime observability endpoint i security header/cookie guardrail-i
- Dodat je in-process background jobs obrazac sa tenancy maintenance job-om
- Definisana je pocetna caching strategija za tenant katalog i read-mostly konfiguraciju
- `Phase 11` je zavrsena
- Uveden je shared module initialization pipeline i `Program.cs` je zadrzan bez module-specific startup grananja
- Dodat je formatting baseline kroz `.editorconfig`, lokalni format check script i CI guardrail
- Dodata je coverage audit beleznica za `Customers` i kriticni tenant isolation test
- Tenant-before-auth redosled je pokriven integration test scenarijem i startup/pipeline cleanup-om
- Uvedeni su membership baseline, shared current execution contract i prvi tenant authorization check na `Customers` modulu
- `Customers` sada nosi i pocetni audit metadata obrazac za `CreatedByUserId`, `ModifiedByUserId`, tenant identitet i UTC timestamp polja
- `Customers` sada ima i prvu tenant role semantiku: `Member`/`Admin` za read, `Admin` za write
- Identity modul sada ima i admin-only server-side tok za tenant membership list/upsert operacije
- Auth state sada iznosi aktivni tenant i tenant rolu do klijenta za tenant-aware UI i dalji authorization flow
- `Phase 12` je zavrsena
- `Phase 13` je aktivna
- Dodat je tenant administration UI za aktivni tenant sa membership list/upsert tokom, role-change formom i guardrail porukom za non-admin korisnike
- Dodat je i tenant catalog/create-provisioning UI baseline za prvi admin flow kreiranja novog tenant-a iz aplikacije
- Sledeci fokus su test scenariji i dalji docs/high-level alignment za tenant administration surface

After that:
- Prosiriti tenant admin surface i otvoriti sledeci business/admin milestone na vec uspostavljenom tenant management temelju
