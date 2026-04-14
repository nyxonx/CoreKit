# Current Roadmap Snapshot

Detaljan roadmap se vodi u `docs/roadmap/README.md`.

## Current Phase

Phase 14 - Platform AppHost Extraction And Control Plane Foundation

## Current Task

Phase 14 je prelomljena tako da vise ne sirimo `platform-admin` unutar postojeceg tenant AppHost-a, nego izdvajamo control-plane deo u poseban platform server + client. Dosadasnji platform UX i backend baseline ostaju korisni, ali se sada tretiraju kao prelazno stanje koje treba preseliti u novi `PlatformAppHost`.

## Next Tasks

- Dodati `CoreKit.PlatformAppHost.Client` i `CoreKit.PlatformAppHost.Server` kao novu host osnovu za `admin.local`
- Premestiti platform login, layout, navigaciju i `platform-admin` page iz tenant client-a u novi platform client
- Odcistiti tenant client od control-plane route-ova, redirect helpera i drugog platform UI grananja
- Razdvojiti bootstrap i konfiguraciju tenant i platform hosta uz zadrzavanje shared modules/contracts osnove
- Nakon ekstrakcije uraditi build verifikaciju oba hosta i uskladiti dokumentaciju

## After That

- Nastaviti platform feature-e tek na stabilnoj dual-AppHost osnovi, bez daljeg mesanja tenant i control-plane UX-a
