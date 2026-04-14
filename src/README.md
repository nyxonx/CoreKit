# Source

Ovaj folder sadrzi produkcioni kod za `CoreKit`.

Trenutna organizacija:

- `AppHosts/`
  - tenant i platform host entrypoint projekti
  - shared host contracts
- `BuildingBlocks/`
  - shared arhitektonske i platformske osnove
- `Modules/`
  - business capability moduli

Pravilo:
- `src/` sadrzi samo produkcione projekte i produkcionu dokumentaciju vezanu za njih
- host entrypoint-i ne treba da budu pomesani sa business modulima na istom nivou kada postoji vise hostova
- bez testova i eksperimentalnih skripti u ovom folderu
