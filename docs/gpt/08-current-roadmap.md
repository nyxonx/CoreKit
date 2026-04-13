# Current Roadmap Snapshot

Detaljan roadmap se vodi u `docs/roadmap/README.md`.

## Current Phase

Phase 14 - Platform Administration Surface And Tenant Lifecycle Management

## Current Task

Phase 14 sada ima prvi control-plane UX slice: zaseban platform layout/login, tenant catalog selection/detail, platform-level membership list/upsert baseline za izabrani tenant i prve activate/deactivate tokove za tenant i membership status.

## Next Tasks

- Dodati test scenarije za control-plane i platform membership/lifecycle tokove tamo gde je prakticno
- Uskladiti README i high-level dokumentaciju sa prosirenim platform administration surface-om
- Nasloniti naredne control-plane tokove na izabrani tenant kontekst unutar `platform-admin` surface-a, ukljucujuci eventualni create-user-to-tenant flow

## After That

- Sledeci business modul i siri admin surface na stabilnijem tenant management temelju
