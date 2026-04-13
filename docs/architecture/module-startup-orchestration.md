# Module Startup Orchestration

Ovaj dokument opisuje zeljeni pravac za startup orkestraciju modula u `CoreKit`.

## Problem

`Program.cs` ne treba da zna detalje pojedinacnih modula.

Kada host direktno poznaje module-specific bootstrap korake, vremenom se:

- povecava coupling izmedju `AppHost.Server` i modula
- startup logika rasipa po host-u
- dodavanje novog modula trazi novu improvizaciju
- teze se proverava da svi moduli prolaze isti startup tok

## Desired Direction

Svaki modul treba da ostane iza shared contract-a `ICoreKitModule` i da koristi isti lifecycle:

- `AddServices(...)`
- `MapEndpoints(...)`
- `InitializeAsync(...)`

Host treba da orkestrira module genericki kroz katalog kao sto je `CoreKitModuleCatalog`, bez grananja po konkretnim modulima.

## Startup Flow

Prakticni cilj je da runtime startup ostane u jednom standardnom obrascu:

1. host ucita registrovane module iz kataloga
2. svi moduli registruju svoje servise kroz shared contract
3. host mapira module endpoint-e genericki
4. host pokrece module initialization kroz zajednicki `InitializeAsync` pipeline

Ovim pristupom startup logika ostaje koncentrisana u module i shared orchestration sloj, umesto da se razbacuje po `Program.cs`.

## Why It Helps

Ovakva orkestracija pomaze zato sto:

- novi modul prati isti obrazac bez dodatnog host-specific wiring-a
- platformski bootstrap ostaje citljiv i odrziv
- modul zadrzava svoju odgovornost za sopstveni startup
- kasnije izdvajanje modula ili jacanje granica ostaje lakse

## Guardrail

Ako je modulu potreban poseban startup korak, on i dalje treba da bude sakriven iza module contract-a ili shared pipeline ekstenzije, a ne da se uvodi kao pojedinacna logika u `Program.cs`.
