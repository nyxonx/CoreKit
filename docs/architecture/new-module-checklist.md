# New Module Checklist

Koristi ovu listu kada dodajes novi modul u `CoreKit`.

## 1. Struktura

- Kreirati `Domain`, `Application`, `Infrastructure` i `Presentation` projekat
- Dodati projekte u `CoreKit.sln`
- Podesiti references tako da ostanu u dozvoljenom smeru

## 2. Granice

- Potvrditi da naziv modula predstavlja jednu poslovnu sposobnost
- Izbeci direktnu zavisnost na drugi modul osim kroz contracts ili eksplicitno odobren integration seam
- Ne izlagati infrastructure tipove klijentu

## 3. Application Slice

- Dodati application assembly marker
- Dodati command/query handlere za prvi use case
- Dodati validatore gde imaju smisla
- Koristiti shared CQRS result model iz `BuildingBlocks.Application`

## 4. Infrastructure

- Registrovati persistence/external integrations u infrastructure projektu
- Dodati bootstrap ili init korak samo ako modul zaista ima startup potrebu
- Ne uvoditi infrastructure logiku u presentation ili application sloj

## 5. Presentation

- Dodati module class koja implementira `ICoreKitModule`
- Mapirati specialized endpoint-e ili se osloniti na RPC kada je use case business operacija
- Dodati presentation-only DI registracije

## 6. Host Wiring

- Dodati modul u [CoreKitModuleCatalog.cs](C:/Users/nikol/source/repos/nyxonx/CoreKit/src/CoreKit.AppHost.Server/Extensions/CoreKitModuleCatalog.cs)
- Potvrditi da se application assembly vidi iz shared pipeline registracije
- Potvrditi da `InitializeAsync` prolazi pri startu

## 7. Client Access

- Ako klijent koristi modul, dodati contracts u `CoreKit.AppHost.Contracts`
- Dodati module-specific client u `CoreKit.AppHost.Client`
- Ne koristiti raw RPC operation stringove direktno u UI komponentama

## 8. Verifikacija

- `dotnet build CoreKit.sln -m:1`
- relevantni `dotnet test`
- proveriti da endpoint-i ili RPC operacije rade kroz AppHost

## 9. Dokumentacija

- Azurirati roadmap status
- Dodati ili prosiriti modulsku README dokumentaciju po potrebi
- Ako modul uvodi novo arhitektonsko pravilo, dopuniti `docs/architecture`
