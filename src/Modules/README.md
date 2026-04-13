# Modules

Ovaj folder sadrzi poslovne module sistema `CoreKit`.

Pravila:
- svaki modul ima svoje `Domain`, `Application`, `Infrastructure` i `Presentation` projekte
- moduli moraju ostati jasno odvojeni
- shared stvari idu u `BuildingBlocks`, ne u drugi modul

Trenutni moduli:
- `Identity` za auth i korisnike
- `Tenancy` za tenant resolution i tenant-aware persistence osnovu
- `Customers` kao prvi pravi poslovni modul
