# Background Jobs

Ovaj dokument definise pocetni pristup za background jobs u `CoreKit`.

## Principles

- Background job mora biti eksplicitno registrovan kroz shared contract.
- Job ne sme da zaobilazi module i njihove servise.
- Job mora biti bezbedan za ponovljeno izvrsavanje.
- Tenant-aware posao mora da koristi tenant-aware servise i da ne mesа tenant podatke.
- Scheduler je za sada in-process i namenjen je platform osnovi, ne teskom batch radu.

## Current Contract

- `ICoreKitBackgroundJob` definise:
  - `Name`
  - `Interval`
  - `ExecuteAsync`
- `AddCoreKitBackgroundJob<TJob>()` registruje job i shared hosted service petlju.
- `CoreKitBackgroundJobHostedService` izvrsava sve registrovane job-ove:
  - jednom odmah pri startu
  - zatim periodicki po njihovom intervalu

## Current Runtime Usage

- `TenantCatalogMaintenanceBackgroundJob`
  - validira tenant konfiguraciju
  - pokrece idempotentni tenant provisioning sync
  - audit loguje uspesno izvrsavanje

## Guardrails

- Dugotrajni ili distribuirani workload-i ne treba da ostanu na ovom in-process modelu.
- Ako job postane kritican za retry, persistence ili scheduling garancije, treba preci na namenski scheduler/queue alat.
- Job logika mora ostati u servisima/modulima; hosted service sloj je samo orkestracija.

## Future Upgrade Path

- Quartz/Hangfire ili zaseban worker proces
- per-job retry policy
- persisted job history
- tenant-scoped batch orchestration
