# Architecture

Ovde ide arhitektonska dokumentacija sistema.

Tipican sadrzaj:
- kontekst sistema
- glavni moduli i njihove odgovornosti
- tokovi podataka
- integracije i granice sistema
- operativne i platform smernice kao sto su module framework i caching strategija
- background jobs pristup i operativni runtime obrasci
- startup orchestration smernice za module i host
- tenant membership i authorization smernice iznad centralnog identity store-a
- tenant AppHost za membership/role upravljanje po aktivnom tenant-u
- platform AppHost za control-plane tenant catalog, provisioning i platform-level administraciju
- coverage audit beleznice za pilot business module kada su bitne za platformske faze

## Trenutni Host Model

CoreKit trenutno koristi dual-AppHost model:

- tenant-facing AppHost:
  - [CoreKit.AppHost.Server](../../src/AppHosts/Tenant/CoreKit.AppHost.Server)
  - [CoreKit.AppHost.Client](../../src/AppHosts/Tenant/CoreKit.AppHost.Client)
- platform control-plane AppHost:
  - [CoreKit.PlatformAppHost.Server](../../src/AppHosts/Platform/CoreKit.PlatformAppHost.Server)
  - [CoreKit.PlatformAppHost.Client](../../src/AppHosts/Platform/CoreKit.PlatformAppHost.Client)

Oba hosta dele:

- [CoreKit.AppHost.Contracts](../../src/AppHosts/Shared/CoreKit.AppHost.Contracts)
- [BuildingBlocks](../../src/BuildingBlocks)
- [Modules](../../src/Modules)

To znaci da poslovna i modulska logika ostaju zajednicke, dok su host bootstrap, routing, layout i administrativni surface-i odvojeni po odgovornosti.

## Ciljni Multi-AppHost Pravac

Trenutni dual-AppHost model nije krajnja granica sistema, vec osnova za buduci model sa vise zasebnih AppHost parova.

Ciljni smer je:

- `PlatformAppHost` ostaje jedini owner tenant kataloga, tenant provisioning-a i tenant lifecycle operacija
- `PlatformAppHost` ostaje jedini owner centralnog identity/user store-a i platform-level membership administracije
- svaki poslovni proizvod ili app surface moze imati svoj poseban AppHost par
  - primeri:
    - `ExpensesAppHost.Server/Client`
    - `MembersAppHost.Server/Client`
    - `CrmAppHost.Server/Client`
- tenant/business AppHost-ovi ne treba dugorocno direktno da zavise od catalog baze
- tenant/business AppHost-ovi ne treba da postanu owner centralnog user store-a
- tenant resolution i runtime tenant podaci treba da idu kroz `TenantRegistry` sloj

`TenantRegistry` pravac je planiran u dve implementacione etape:

- lokalni adapter:
  - prelazna implementacija iza interfejsa, korisna za lokalni razvoj i postepenu ekstrakciju
  - trenutni baseline je uveden kroz `ITenantRegistry` + `TenantCatalogTenantRegistry`
  - `TenantResolutionService` vise ne zavisi direktno od `TenantCatalogDbContext`, vec od registry contract-a
- remote API model:
  - `PlatformAppHost` izlaze tenant registry endpoint-e
  - tenant/business hostovi tenant informacije dobijaju preko registry client-a umesto direktnim citanjem catalog persistence sloja

Time se izbegava povratak na jedan shared mega-AppHost, a buduci AppHost parovi dobijaju jasnu i ponovljivu osnovu.
