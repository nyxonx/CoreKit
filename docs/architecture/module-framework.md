# Module Framework

Ovaj dokument opisuje standardni obrazac za module u `CoreKit`.

## Cilj

Svaki novi modul treba da moze da se uvede bez improvizacije kroz isti skup koraka:

- shared contracts kada su potrebni ka klijentu
- application assembly za use case logiku
- infrastructure registraciju i bootstrap
- presentation layer koji mapira endpoint-e
- module class koja povezuje sve zajedno

## Standardni Oblik Modula

Svaki modul koristi istu strukturu:

```text
/src/Modules/[Module]
    /CoreKit.Modules.[Module].Domain
    /CoreKit.Modules.[Module].Application
    /CoreKit.Modules.[Module].Infrastructure
    /CoreKit.Modules.[Module].Presentation
```

## Module Contract

Shared contract za host registraciju je `ICoreKitModule` u [ICoreKitModule.cs](C:/Users/nikol/source/repos/nyxonx/CoreKit/src/BuildingBlocks/CoreKit.BuildingBlocks.Presentation/ICoreKitModule.cs).

Svaki modul preko njega definise:

- `Name`
- `ApplicationAssemblies`
- `AddServices(...)`
- `MapEndpoints(...)`
- `InitializeAsync(...)`

To omogucava da `AppHost.Server` ne zna detalje svakog modula, vec samo enumerira module iz [CoreKitModuleCatalog.cs](C:/Users/nikol/source/repos/nyxonx/CoreKit/src/CoreKit.AppHost.Server/Extensions/CoreKitModuleCatalog.cs).

## Runtime Tok

Standardni tok je:

1. AppHost ucita katalog modula
2. svi moduli registruju svoje servise
3. AppHost registruje shared CQRS/RPC pipeline nad modulskim application assembly-jima
4. svi moduli mapiraju svoje endpoint-e
5. shared initialization pipeline izvrsava module bootstrap kroz `InitializeAsync`

Trenutni shared startup orchestration je dokumentovan i u `docs/architecture/module-startup-orchestration.md`.

## Trenutni Primeri

Postoje dva referentna primera:

- [IdentityModule.cs](C:/Users/nikol/source/repos/nyxonx/CoreKit/src/Modules/Identity/CoreKit.Modules.Identity.Presentation/IdentityModule.cs)
- [TenancyModule.cs](C:/Users/nikol/source/repos/nyxonx/CoreKit/src/Modules/Tenancy/CoreKit.Modules.Tenancy.Presentation/TenancyModule.cs)

`Identity` pokazuje modul sa specijalizovanim auth endpoint-ima.

`Tenancy` pokazuje modul koji koristi i:

- runtime tenant infrastructure
- CQRS/RPC tok
- klijentski `ModuleClient` obrazac

## Pravilo Za Client Access

Kada modul ima klijentski pristup iz WASM-a, koristi se obrazac:

- shared DTO/request/contracts u `CoreKit.AppHost.Contracts`
- modul-specificki client servis u `CoreKit.AppHost.Client`
- `RpcClient` kao transportni mehanizam kada je operacija business use case

Primer:

- [TenancyModuleClient.cs](C:/Users/nikol/source/repos/nyxonx/CoreKit/src/CoreKit.AppHost.Client/Services/TenancyModuleClient.cs)
- [TenancyRpcOperations.cs](C:/Users/nikol/source/repos/nyxonx/CoreKit/src/CoreKit.AppHost.Contracts/Tenancy/TenancyRpcOperations.cs)

## Kada Ne Forsirati RPC

Ne mora svaki modul da koristi `POST /api/rpc` za sve.

Platformski ili framework-specifik endpoint-i mogu ostati odvojeni kada to pojednostavljuje integraciju, kao sto je auth tok u `Identity` modulu.

Bitno je da modul i dalje prati isti registration/startup obrazac i da klijent ima jasan pristupni servis.
