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
