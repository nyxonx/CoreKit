# BuildingBlocks

Ovaj folder sadrzi shared building blocks za `CoreKit`.

Namena:
- zajednicke apstrakcije
- bazne tipove
- cross-cutting pomocne komponente
- zajednicku pipeline i infrastructure osnovu

Pravilo:
- ovde ide samo ono sto je stvarno shared izmedju vise modula ili host projekata
- ne prebacivati domensku logiku modula u `BuildingBlocks`

Napomena:
- `CoreKit.BuildingBlocks.Domain` i `CoreKit.BuildingBlocks.Infrastructure` trenutno namerno ostaju lagani i ne dobijaju bazne tipove unapred
- shared primitive kao sto su `Entity<T>`, `ValueObject` ili slicne osnove uvode se tek kada se stvarno pojavi ponovljeni obrazac kroz vise modula
