# Gold Digger API - GD_API

## Beschreibung
Eine einfache ASP.NET Core Web API zur Verwaltung von Schürfrechten, die auf Daten einer MySQL-Datenbank zugreift. Unterstützt CRUD-Operationen für Claims.
Diese API wurde während meiner Ausbildung erstellt und wird in Zukunft weiterentwickelt.

## Endpunkte
| Methode | Route                                    | Beschreibung                    |
|---------|------------------------------------------|---------------------------------|
|   GET   | /Claims                                  | Alle vorhandenen Claims abrufen |
|   GET   | /Claims/{id}                             | Claim über ID abrufen           |
|   GET   | /Claims/search?claimNumber={ClaimNumber} | Claim über ClaimNumber abrufen  |
|   POST  | /Claims                                  | Neues Schürfrecht erstellen     |
|   PUT   | /Claims/{id}                             | Claim aktualisieren             |
|  DELETE | /Claims/{id}                             | Claim löschen                   |

## Setup & Start
1.	**API starten:**
	```bash
	dotnet run
 	```
### TODO
- Dependency Injection einbauen
- Bessere Validierungen und Exception-Handling
- async
