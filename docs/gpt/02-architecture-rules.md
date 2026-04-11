# Architecture Rules

## Core Principles

- Drzati domensku logiku odvojenu od infrastrukturnih detalja
- Favorizovati jasne granice izmedju modula
- Minimizovati coupling izmedju projekata i slojeva

## Code Organization

- Novi kod smestati u odgovarajuci projekat unutar `src/`
- Testove drzati iskljucivo unutar `tests/`
- Dokumentaciju azurirati kada se menjaju bitne odluke

## Quality Bar

- Kod treba da bude citljiv i testabilan
- Javne API granice treba dokumentovati
- Pre znacajnih promena zapisati odluku kroz ADR kada ima tradeoff-a
