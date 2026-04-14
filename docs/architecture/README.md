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
  - [CoreKit.AppHost.Server](../../src/CoreKit.AppHost.Server)
  - [CoreKit.AppHost.Client](../../src/CoreKit.AppHost.Client)
- platform control-plane AppHost:
  - [CoreKit.PlatformAppHost.Server](../../src/CoreKit.PlatformAppHost.Server)
  - [CoreKit.PlatformAppHost.Client](../../src/CoreKit.PlatformAppHost.Client)

Oba hosta dele:

- [CoreKit.AppHost.Contracts](../../src/CoreKit.AppHost.Contracts)
- [BuildingBlocks](../../src/BuildingBlocks)
- [Modules](../../src/Modules)

To znaci da poslovna i modulska logika ostaju zajednicke, dok su host bootstrap, routing, layout i administrativni surface-i odvojeni po odgovornosti.
