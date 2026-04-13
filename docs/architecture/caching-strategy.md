# Caching Strategy

Ovaj dokument definise pocetnu caching strategiju za `CoreKit`.

## Principles

- Cache nikad ne sme da probije tenant granice.
- Tenant-aware podatak mora da koristi cache key koji ukljucuje tenant identitet.
- Write tokovi ne smeju da zavise od cache-a za ispravnost.
- Cache je optimizacija, ne izvor istine.
- Kratki TTL i jasna invalidacija imaju prioritet nad agresivnim kesiranjem.

## Current Usage

- `TenantResolutionService` koristi in-memory cache za tenant katalog lookup po:
  - `identifier`
  - `host`
- TTL je konfigurabilan preko `Caching:TenantCatalog:TtlSeconds`.
- Kesiraju se samo pozitivni lookup rezultati.

## Allowed Early Caching Targets

- Tenant katalog i drugi read-mostly konfiguracioni podaci
- Read modeli koji imaju:
  - jasan tenant key
  - kontrolisanu invalidaciju
  - mali rizik od kratkog staleness prozora

## Not Allowed By Default

- Auth state i security-sensitive odluke
- Write command rezultati
- Tenant poslovni podaci bez tenant-aware key-a
- Shared cache entry koji ne razdvaja `localhost`, `contoso`, `fabrikam` i ostale tenant-e

## Next Steps

- Ako se uvede cache za module kao sto je `Customers`, mora da postoji:
  - tenant-aware cache key schema
  - invalidacija posle create/update/delete operacija
  - jasan TTL i dokumentovan stale-data tradeoff
