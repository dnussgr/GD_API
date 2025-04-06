# Gold Digger API - GD_API

## Beschreibung
Eine ASP.NET Core Web API zur Verwaltung von Schürfrechten. Unterstützt CRUD-Operationen für Claims.

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