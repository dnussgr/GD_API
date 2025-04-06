using GD_API.Data;
using GD_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace GD_API.Controllers;

[ApiController]         // Definiert Klasse als API-Controller
[Route("[controller]")] // Routing -> legt fest, unter welcher URL der Controller erreichbar ist. "Controller" wird autom. durch Klassennamen ersetzt
public class ClaimsController : ControllerBase // Erbt API Funktionen wie Ok() oder NotFound() von ControllerBase 
{

    private readonly ClaimRepository _claimRepository;
    public ClaimsController(ClaimRepository claimRepository) 
    {
        _claimRepository = claimRepository;
    }


    /// <summary>
    /// GET-Anfrage für alle Schürfrechte
    /// </summary>
    /// <returns>Alle gefundenen Elemente</returns>
    [HttpGet]
    public IActionResult GetAllClaims() => Ok(_claimRepository.GetAllClaims());
    // IActionResult ist der Return-Type für API Methoden, hier -> Ok(), Ok (Code 200) steht für eine erfolgreiche Durchführung


    /// <summary>
    /// Sucht Schürfrecht über eine ID
    /// </summary>
    /// <param name="id">ID des Claims auf der Datenbank</param>
    /// <returns>Das über die ID gefundene Element</returns>
    [HttpGet("{id}")]
    public IActionResult GetClaimById(int id)
    {
        var claim = _claimRepository.GetClaimById(id);
        return claim != null ? Ok(claim) : NotFound($"Kein Schürfrecht mit der ID {id} gefunden.");
    }


    /// <summary>
    /// Sucht Schürfrecht über ClaimNumber
    /// </summary>
    /// <param name="claimNumber">ClaimNumber des Schürfrechts</param>
    /// <returns>Das über die ClaimNumber gefundene Element</returns>
    [HttpGet("search")]
    public IActionResult GetByClaimNumber([FromQuery] string claimNumber)
    {
        var claim = _claimRepository.GetByClaimNumber(claimNumber);
        return claim != null ? Ok(claim) : NotFound($"Kein Schürfrecht mit der Nummer {claimNumber} gefunden.");
    }


    /// <summary>
    /// Erstellt ein neues Schürfrecht auf der Datenbank
    /// </summary>
    /// <param name="claim">Schürfrecht-Objekt</param>
    /// <returns>Bestätigung der Erstellung</returns>
    [HttpPost]
    public IActionResult CreateClaim(Claim claim)
    {
        if (string.IsNullOrWhiteSpace(claim.ClaimNumber) ||
            string.IsNullOrWhiteSpace(claim.OwnerName) ||
            string.IsNullOrWhiteSpace(claim.Location))
        {
            return BadRequest( new { message = "Alle Felder müssen befüllt sein" });
        }

        if (claim.GoldAmount <= 0)
            return BadRequest(new { message = "Goldmenge muss über 0 sein." });

        if (claim.DateClaimed == default(DateTime) || claim.DateClaimed > DateTime.Now)
            return BadRequest(new { message = "Datum darf weder leer sein, noch in der Zukunft liegen." });

        int newClaimId = _claimRepository.CreateClaim(claim);
        if (newClaimId > 0)
            return CreatedAtAction(nameof(GetClaimById), new { id = newClaimId }, claim);

        return StatusCode(500, "Fehler beim ERstellen des Schürfrechts.");
    }


    /// <summary>
    /// Aktualisiert die Daten eines Schürfrechts
    /// </summary>
    /// <param name="id">ID auf der Datenbank</param>
    /// <param name="claim">Schürfrecht-Objekt</param>
    /// <returns>Bestätigung der erfolgreichen Änderung</returns>
    [HttpPut("{id}")]
    public IActionResult UpdateClaim(int id, Claim claim)
    {
        if (!_claimRepository.UpdateClaim(id, claim))
            return NotFound($"Schürfrecht mit der ID {id} nicht gefunden. Aktualisierung nicht durchgeführt.");

        return Ok($"Schürfrecht mit der ID {id} wurde erfolgreich aktualisiert");
    }


    /// <summary>
    /// Löscht einen Schürfrecht-Datensatz über ID
    /// </summary>
    /// <param name="id">ID des Schürfrechts auf der Datenbank</param>
    /// <returns>Bestätigung der erfolgreichen Löschung</returns>
    [HttpDelete("{id}")]
    public IActionResult DeleteClaim(int id)
    {
        if (!_claimRepository.DeleteClaim(id))
            return NotFound($"Das zu löschende Schürfrecht mit der ID {id} konnte nicht gefunden werden.");
        return Ok($"Schürfrecht mit der ID {id} wurde erfolgreich gelöscht.");
    }
}
