# Tenant Membership And Authorization

Ovaj dokument opisuje pocetni `Phase 12` obrazac za tenant-scoped membership i authorization iznad centralizovanog Identity store-a.

## Cilj

CoreKit trenutno zadrzava:

- centralizovan `AppUser` identitet u jednom Identity store-u
- `database-per-tenant` za business podatke

Izmedju ta dva sloja uvodi se tenant membership model koji odredjuje:

- kom tenant-u korisnik pripada
- da li je membership aktivan
- koju tenant-scoped rolu ili permission osnovu korisnik nosi

## Osnovni Obrazac

Pocetna implementacija uvodi:

- `AppUserTenantMembership` kao vezu izmedju `AppUser` i tenant identifikatora
- shared `CurrentExecutionContext` contract u `BuildingBlocks.Application`
- shared `ICurrentTenantAuthorizationService` za proveru pristupa aktivnom tenant-u
- pocetni audit metadata obrazac za business entitete (`CreatedByUserId`, `ModifiedByUserId`, `TenantIdentifier`, `CreatedUtc`, `ModifiedUtc`)

To znaci da application handler-i mogu da proveravaju pristup bez direktnog znanja o `HttpContext` ili konkretnom Identity persistence detalju.

## Trenutna Podela Odgovornosti

- `Identity.Infrastructure` cita korisnika iz request konteksta i membership podatke iz centralnog Identity store-a
- `Tenancy` i dalje razresava aktivni tenant pre auth i business toka
- business moduli koriste shared authorization contract kada operacija zahteva tenant membership

Host ne implementira module-specific membership pravila.
Pravila ostaju iza shared contract-a i module use case sloja.

## Prvi Primenjeni Slice

Prvi primenjeni authorization slice je `Customers`.

Za `Customers` RPC operacije:

- anonimni korisnik dobija `authentication_required`
- autentikovan korisnik bez membership-a za aktivni tenant dobija `tenant_membership_required`
- `Member` i `Admin` mogu da citaju customer podatke
- `Admin` je potreban za create/update/delete operacije
- korisnik sa neodgovarajucom tenant rolom dobija `tenant_role_required`
- create/update tok dopunjava audit metadata polja iz current execution konteksta

Audit timestamp polja se cuvaju kao UTC vrednosti.
Lokalne vremenske zone i tenant-specific prikaz ostaju briga klijenta ili presentation sloja, ne persistence izvora istine.

To je namerno mali prvi korak.
Tenant-scoped permissions i finije role provere dolaze posle ove osnove, ali prva razlikovana role semantika vec postoji kroz `Member` naspram `Admin`.

## Zasto Ovako

Ovaj pristup omogucava:

- jedan centralni login identitet za vise tenant-a
- tenant-scoped authorization bez dupliranja korisnika po tenant bazi
- jasan put ka buducim audit poljima kao sto su `CreatedByUserId`, `ModifiedByUserId` i `TenantId`
- postepeno uvodjenje role/permission modela bez velikog redesign-a auth sloja
