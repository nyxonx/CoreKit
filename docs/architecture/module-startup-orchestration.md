# Module Startup Orchestration

Ovaj dokument opisuje zeljeni pravac za startup orkestraciju modula u `CoreKit`.

## Problem

`Program.cs` ne treba da zna detalje pojedinacnih modula.

Kada host direktno poznaje module-specific bootstrap korake, vremenom se:

- povecava coupling izmedju host entrypoint-a i modula
- startup logika rasipa po host-u
- dodavanje novog modula trazi novu improvizaciju
- teze se proverava da svi moduli prolaze isti startup tok

## Desired Direction

Svaki modul treba da ostane iza shared contract-a `ICoreKitModule` i da koristi isti lifecycle:

- `AddServices(...)`
- `ConfigurePipeline(...)`
- `MapEndpoints(...)`
- `InitializeAsync(...)`

Host treba da orkestrira module genericki kroz katalog kao sto je `CoreKitModuleCatalog`, bez grananja po konkretnim modulima ili po tome da li je host tenant-facing ili platform-facing.

## Startup Flow

Prakticni cilj je da runtime startup ostane u jednom standardnom obrascu:

1. host ucita registrovane module iz kataloga
2. svi moduli registruju svoje servise kroz shared contract
3. moduli po potrebi prikljucuju svoje middleware/pipeline korake kroz shared hook
4. host mapira module endpoint-e genericki
5. host pokrece module initialization kroz zajednicki `CoreKitModuleInitializationPipeline`

Ovim pristupom startup logika ostaje koncentrisana u module i shared orchestration sloj, umesto da se razbacuje po `Program.cs`.

## Current Shape

Trenutni oblik nakon izdvajanja platform host-a je:

- i tenant i platform server delegiraju startup bootstrap kroz shared ekstenzije i module catalog
- shared pipeline enumerira `ICoreKitModule` instance iz kataloga
- host vise ne treba da zna module-specific middleware detalje kada ih modul prikljucuje kroz `ConfigurePipeline(...)`
- svaki modul i dalje zadrzava sopstveni `InitializeAsync(...)` bez host-specific grananja

Tenant i platform host mogu i dalje imati razlicite spoljne route/layout/bootstrap potrebe, ali modulski startup tok treba da ostane isti.

To zadrzava postojeci CoreKit pravac, ali uklanja potrebu da host bootstrap raste kroz module-specific startup odluke.

## Why It Helps

Ovakva orkestracija pomaze zato sto:

- novi modul prati isti obrazac bez dodatnog host-specific wiring-a
- platformski bootstrap ostaje citljiv i odrziv
- modul zadrzava svoju odgovornost za sopstveni startup
- kasnije izdvajanje modula ili jacanje granica ostaje lakse

## Guardrail

Ako je modulu potreban poseban startup korak, on i dalje treba da bude sakriven iza module contract-a ili shared pipeline ekstenzije, a ne da se uvodi kao pojedinacna logika u `Program.cs`.
