# Build

Ovaj folder je rezervisan za build, provisioning i deployment skripte.

Tipican sadrzaj:
- lokalne bootstrap skripte
- CI/CD pomocne skripte
- migration runner alati
- deployment helper-i

Trenutno sadrzi:
- `provision-local.ps1` za lokalno pokretanje catalog i tenant provisioning toka bez dizanja web host-a

Tipican lokalni poziv:

```powershell
.\build\provision-local.ps1
```
