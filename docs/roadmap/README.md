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

Status: Complete

Tasks:

- `[x]` Dodati prvi tenant administration ekran za aktivni tenant
- `[x]` Povezati membership list/upsert server tokove na Blazor client kroz module client obrazac
- `[x]` Dodati UI za promenu tenant role po korisniku bez ručnog RPC testiranja
- `[x]` Dodati UI za kreiranje novog tenant-a i provisioning flow
- `[x]` Dodati pregled aktivnog tenant konteksta u admin povrsini
- `[x]` Dodati osnovni guardrail UX za non-admin korisnike na admin ekranima
- `[x]` Dodati test scenarije za tenant administration UI tokove tamo gde je prakticno
- `[x]` Uskladiti README i high-level dokumentaciju sa tenant administration UI baseline-om

Exit criteria:

- Postoji tenant administration UI koji koristi postojece server-side tokove
- Tenant admin moze da vidi i menja membership/role za aktivni tenant
- Tenant creation/provisioning ima jasan UI ili proverljiv flow
- Admin povrsina jasno razlikuje tenant admin i non-admin iskustvo

---

## Phase 14 - Platform AppHost Extraction And Control Plane Foundation

Goal:
Smanjiti mesanje tenant i platform scope-a tako sto se control-plane deo izdvaja iz postojecg tenant AppHost-a u poseban `PlatformAppHost` server + client par, uz zadrzavanje zajednickih modula, contracts sloja i backend capability-ja koji su vec uvedeni.

Status: Completed

Why this phase exists:

- Trenutni `platform-admin` tokovi rade, ali su uvedeni unutar istog AppHost-a koji hostuje tenant UX
- Tenant i platform routing, login i auth shaping su poceli da se preplicu kroz specijalne slucajeve
- Dalje dodavanje feature-a u istom hostu bi povecalo coupling i usporilo buduci razvoj
- Potrebna je cistija osnova pre sledeceg talasa platform administracije

Implementation strategy:

1. Zadrzati postojeci `CoreKit.AppHost.Server` i `CoreKit.AppHost.Client` kao tenant-facing host tokom tranzicije
2. Uvesti novi `CoreKit.PlatformAppHost.Server` i `CoreKit.PlatformAppHost.Client`
3. Premestiti platform login/layout/navigation/page flow u novi platform client
4. Zadrzati `BuildingBlocks`, `Modules` i `CoreKit.AppHost.Contracts` kao shared osnovu
5. Ocistiti tenant host od control-plane UI grananja i platform ruta nakon uspesne ekstrakcije

Tasks:

- `[x]` Dodati `CoreKit.PlatformAppHost.Client` projekat kao poseban Blazor WebAssembly client za control-plane UI
- `[x]` Dodati `CoreKit.PlatformAppHost.Server` projekat koji hostuje samo platform client i njemu potreban server bootstrap
- `[x]` Uvesti platform AppHost bootstrap extensione i konfiguraciju odvojeno od tenant AppHost bootstrapa tamo gde to ima smisla
- `[x]` Premestiti `platform-admin` page, platform login, platform layout i platform navigaciju iz tenant client-a u novi platform client
- `[x]` Premestiti platform-specifine client servise i registracije u novi platform client, uz zadrzavanje shared RPC/contracts osnove
- `[x]` Ukloniti platform route handling i control-plane UI branching iz tenant client-a nakon ekstrakcije
- `[x]` Svesti tenant client ponovo na tenant-only UX: tenant login, tenant home i tenant administraciju aktivnog tenant-a
- `[x]` Podesiti lokalni development setup i host mapiranje tako da `admin.local` gadja platform AppHost, a tenant hostovi tenant AppHost
- `[x]` Potvrditi da postojece platform backend capability-je ostaju dostupne kroz novi platform host bez ponovnog mesanja sa tenant hostom
- `[x]` Uskladiti roadmap, README i high-level arhitekturu sa dual-AppHost modelom
- `[x]` Dodati build verifikaciju za oba hosta i kasnije test scenarije tamo gde je prakticno
- `[x]` Proci zavrsni stabilization pass kroz test scenarije i preostali dual-host smoke check kada lokalni test run bude stabilan

Suggested execution slices:

- `Phase 14A - Extraction Skeleton`
  - Zavrseno
  - Dodati nova platform client/server projekta
  - Uvesti minimalni bootstrap i local launch konfiguraciju
  - Potvrditi da `admin.local` moze da sluzi novi platform host
- `Phase 14B - Platform UI Migration`
  - Zavrseno
  - Premestiti platform Razor fajlove, layout i login flow
  - Premestiti platform client servise i DI registracije
  - Ostaviti tenant UI bez platform route-ova
- `Phase 14C - Host Cleanup And Separation`
  - Zavrseno
  - Ocistiti tenant AppHost od `IsControlPlaneHost` UI workaround-a
  - Razdvojiti konfiguraciju i bootstrap gde je potrebno
  - Proveriti da middleware i auth shaping vise ne nose nepotreban UI coupling
- `Phase 14D - Stabilization`
  - Zavrseno
  - Proci build verifikaciju oba hosta
  - Uskladiti dokumentaciju
  - Ostaviti preostale testove i dual-host smoke check kao zavrsni cleanup
  - Ostaviti sledece platform feature-e za narednu fazu

Out of scope for this phase:

- Novi veliki platform feature-i izvan vec postojeceg baseline-a
- `Create user + assign tenant` flow
- Dublji UX polish izvan onoga sto je potrebno da ekstrakcija bude cista i upotrebljiva
- Siri admin/business milestone-i koji nisu direktno vezani za razdvajanje hostova

Exit criteria:

- Postoji poseban platform AppHost server + client za control-plane iskustvo
- Tenant AppHost vise ne sadrzi platform route-ove, platform login ni platform layout
- `admin.local` koristi platform host, a tenant hostovi tenant host
- Shared modules i contracts ostaju jedinstveni bez dupliranja poslovne logike
- Arhitektura je dovoljno cista da sledeca faza moze da doda platform feature-e bez novog mesanja sa tenant UX-om

---

## Phase 15 - AppHost Solution Structure Cleanup

Goal:
Preurediti `src` strukturu tako da tenant i platform host projekti budu grupisani pod zajednicki `AppHosts` folder, uz minimalan rizik i bez promene poslovne logike ili odgovornosti uvedenih u `Phase 14`.

Status: Completed

Tasks:

- `[x]` Premestiti tenant host projekte pod `src/AppHosts/Tenant`
- `[x]` Premestiti platform host projekte pod `src/AppHosts/Platform`
- `[x]` Premestiti `CoreKit.AppHost.Contracts` pod `src/AppHosts/Shared`
- `[x]` Azurirati `CoreKit.sln` i project reference putanje prema novoj strukturi
- `[x]` Azurirati docs koje referenciraju stare AppHost putanje
- `[x]` Potvrditi da `dotnet build CoreKit.sln -m:1` prolazi nakon restrukturiranja
- `[x]` Odraditi kratki smoke check za tenant i platform startup nakon move-a

Exit criteria:

- Svi AppHost projekti zive pod `src/AppHosts`
- `src` je organizovan po odgovornosti: `AppHosts`, `BuildingBlocks`, `Modules`
- Build prolazi bez rucnog krpljenja putanja
- Dokumentacija prati novu strukturu

---

## Phase 16 - Platform AppHost Cleanup And Responsibility Tightening

Goal:
Procistiti `PlatformAppHost` posle razdvajanja hostova tako da platform client i platform server nose samo ono sto im stvarno pripada, bez tenant-side tranzicionih ostataka i bez nepotrebno sirokih shared DTO i auth surface-a.

Status: Completed

Tasks:

- `[x]` Auditirati `PlatformAppHost.Client` page/layout/service fajlove i evidentirati tranzicione ostatke
- `[x]` Auditirati `PlatformAppHost.Server` bootstrap, diagnostics i RPC surface i izdvojiti sta treba ostati platform-specific, a sta je samo prelazni copy
- `[x]` Procistiti platform auth state i auth DTO upotrebu tako da platform client koristi samo podatke koji mu realno trebaju
- `[x]` Ukloniti tenant-context i control-plane workaround logiku iz platform UI-a tamo gde je sada suvisna
- `[x]` Uskladiti naming i UI copy da vise ne zvuci kao tranzicija iz starog zajednickog AppHost-a
- `[x]` Potvrditi da build prolazi posle cleanup-a
- `[x]` Ostaviti tenant AppHost cleanup kao poseban sledeci slice nakon zavrsenog platform cleanup-a

Exit criteria:

- `PlatformAppHost.Client` i `PlatformAppHost.Server` imaju jasniju i uzu odgovornost
- Platform auth i shared contracts ne nose nepotrebne tenant/control-plane ostatke bez razloga
- Platform server bootstrap i copied infrastructure su pregledani i svedeni na opravdan minimum
- Build prolazi i cleanup ne uvodi regresiju u platform host ponasanje
- Lokalni tenant/catalog/identity SQLite fajlovi vise ne zive po pojedinacnim AppHost folderima nego u zajednickom `localdata/` prostoru

---

## Phase 17 - Local Database Configuration Cleanup

Goal:
Procistiti lokalnu SQLite konfiguraciju tako da `Tenancy:DatabaseRoot` bude jedini izvor za folder u kome zive baze, dok se kroz config kljuceve navode samo imena fajlova i centralno sastavljaju connection stringovi za catalog, identity, seed i provisioning tokove.

Status: Completed

Tasks:

- `[x]` Uvesti centralni resolver za lokalne SQLite connection stringove
- `[x]` Prebaciti tenant catalog i identity bootstrap da se naslanjaju na `DatabaseRoot`
- `[x]` Prebaciti seed i create-tenant provisioning tokove da grade connection stringove iz `DatabaseRoot`
- `[x]` Ukloniti dupliranje `../../../../localdata/...` iz host `appsettings.json` fajlova
- `[x]` Potvrditi da build prolazi posle cleanup-a
- `[x]` Rucno proveriti da novi tenant i dalje kreira bazu u `localdata/`

Exit criteria:

- `Tenancy:DatabaseRoot` je jedini izvor za lokalni folder baza
- Host `appsettings.json` ne dupliraju putanju kroz pune relativne SQLite connection stringove
- Tenant catalog, identity, seed i provisioning koriste isto pravilo za lokalne SQLite putanje
- Build prolazi i novi tenant vise ne zavrsava bazu u AppHost folderu
- Startup vise ne seeduje `bootstrap`, `contoso` ni `fabrikam`, vec katalog krece prazan dok se tenant-i ne kreiraju kroz platform admin

---

## Phase 18 - Tenant Registry Decoupling And Multi-AppHost Foundation

Goal:
Uspostaviti arhitekturu u kojoj je `PlatformAppHost` jedini owner tenant kataloga, dok tenant-facing i buduci business AppHost-ovi tenant informacije dobijaju kroz `TenantRegistry` sloj umesto kroz direktan pristup catalog bazi, uz jasno definisan i planiran put i za buduci remote API model.

Status: In Progress

Tasks:

- `[x]` Definisati ciljnu arhitekturu i granice odgovornosti izmedju `PlatformAppHost` i tenant/business AppHost-ova
- `[x]` Definisati `TenantRegistry` contract, read modele i pravila za tenant resolution bez direktnog oslanjanja na catalog DB
- `[ ]` Definisati granicu izmedju centralnog identity/user ownership-a i tenant/business host potrosnje auth i membership podataka
- `[x]` Isplanirati lokalni adapter kao prelaznu implementaciju iza `TenantRegistry` interfejsa
- `[~]` Definisati remote registry API podfazu tako da `PlatformAppHost` ostane jedini owner tenant kataloga
- `[~]` Definisati host wiring i template smernice za buduce AppHost parove (`Expenses`, `Members`, `CRM`, itd.)
- `[>]` Uskladiti architecture docs i roadmap sa novim modelom

Suggested execution slices:

- `Phase 18A - Architecture And Boundaries`
  - Definisati sta je owner `PlatformAppHost`, a sta je odgovornost tenant/business hostova
  - Dokumentovati sta hostovi smeju i ne smeju da znaju direktno
  - Definisati da `PlatformAppHost` ostaje owner centralnog identity/user store-a i platform membership administracije
- `Phase 18B - Tenant Registry Contract`
  - Zavrseno
  - Uvesti `ITenantRegistry`
  - Definisati DTO/read model za resolution i tenant runtime podatke
- `Phase 18C - Local Adapter`
  - Zapoceto
  - Zadrzati DB-backed adapter kao lokalnu/prelaznu implementaciju iza interfejsa
  - Osloboditi tenant host direktnog znanja o catalog persistence detaljima
- `Phase 18D - Remote Registry API`
  - Zapoceto
  - Definisati endpoint-e i client adapter za buduci remote registry tok
  - Ostaviti `PlatformAppHost` kao jedini owner tenant kataloga i lifecycle operacija
- `Phase 18E - Host Wiring And Stabilization`
  - Zapoceto
  - Definisati obrazac za buduce AppHost parove
  - Proveriti build i osnovni smoke/stabilization tok kad implementacija bude gotova

Exit criteria:

- `PlatformAppHost` je jasno definisan kao jedini owner tenant kataloga i tenant lifecycle operacija
- `PlatformAppHost` je jasno definisan kao owner centralnog identity/user store-a i platform-level membership administracije
- Tenant/business AppHost-ovi vise ne zavise arhitektonski od direktnog pristupa catalog bazi
- Tenant/business AppHost-ovi ne postaju owner ni tenant kataloga ni centralnog user store-a
- Postoji jasan `TenantRegistry` contract sa planom za lokalni adapter i remote API model
- Buduci AppHost parovi imaju dokumentovan obrazac bez vracanja na shared mega-AppHost pristup

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
- `Phase 13` je zavrsena
- Dodat je tenant administration UI za aktivni tenant sa membership list/upsert tokom, role-change formom i guardrail porukom za non-admin korisnike
- Tenant catalog/create-provisioning flow je izdvojen na control-plane platform admin surface umesto da zivi na svakom tenant hostu
- Dodati su integration testovi i high-level docs alignment za tenant administration baseline
- `Phase 14` je zavrsena
- Izdvojen je poseban `CoreKit.PlatformAppHost.Server` + `CoreKit.PlatformAppHost.Client`
- `platform-admin`, platform login, platform layout i platform servisi su preseljeni u novi platform host
- Tenant AppHost je vracen na tenant-only UX bez platform ruta i control-plane UI grananja
- `admin.local` sada predstavlja control-plane host, dok tenant hostovi ostaju na tenant AppHost strani
- README, architecture docs i solution structure snapshot su uskladjeni sa dual-AppHost modelom
- Build verifikacija prolazi i rucni dual-host smoke check je potvrdio da tenant i platform surface rade odvojeno kako je planirano
- `Phase 15` je zavrsena
- Tenant i platform host projekti su preseljeni pod `src/AppHosts`, a `CoreKit.AppHost.Contracts` pod `src/AppHosts/Shared`
- `CoreKit.sln`, module/test project reference putanje i docs su uskladjeni sa novom strukturom
- Build prolazi i startup/smoke proverom je potvrdeno da nova `src/AppHosts` struktura ne uvodi path ili config regresije
- `Phase 16` je zavrsena
- `PlatformAppHost` client i server su procisceni od tranzicionih tenant/control-plane ostataka
- Platform auth koristi uzi `PlatformAuthStateResponse`, a platform host vise ne registruje tenant-facing `Customers` modul
- Lokalni SQLite development fajlovi su prebaceni u zajednicki `localdata/` folder umesto da se dupliraju po AppHost projektima
- `Phase 17` je zavrsena
- `Tenancy:DatabaseRoot` je sada jedini izvor za lokalni folder baza, a host config cuva samo imena fajlova
- Startup vise ne seeduje demo tenant-e, pa clean run pocinje bez tenant-a dok se prvi tenant ne kreira kroz `platform-admin`
- Build prolazi i create-tenant smoke proverom je potvrdeno da nove tenant baze zavrsavaju u `localdata/`
- `Phase 18` je aktivna
- Sledeci fokus je tenant registry decoupling tako da `PlatformAppHost` ostane jedini owner tenant kataloga
- `TenantResolutionService` vise ne zavisi direktno od `TenantCatalogDbContext`, vec od `ITenantRegistry`
- Uvedeni su `TenantRuntimeInfo` read model i lokalni `TenantCatalogTenantRegistry` adapter kao prvi rez
- `PlatformAppHost` sada izlaze host-gated tenant registry endpoint-e kao prvi remote contract korak
- Tenant host je konfigurisan da koristi `RemoteTenantRegistry`, dok platform host ostaje na lokalnom registry owner modu
- Treba eksplicitno definisati i da centralni identity/user store i membership ownership ostaju na platform strani
- Remote registry API nije poseban kasniji haoticni dodatak, nego planirana podfaza iste arhitektonske teme

After that:
- Krenuti u implementacione podfaze za tenant registry i multi-AppHost foundation
