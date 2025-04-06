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
    /// GET-Anfrage f�r alle Sch�rfrechte
    /// </summary>
    /// <returns>Alle gefundenen Elemente</returns>
    [HttpGet]
    public IActionResult GetAllClaims() => Ok(_claimRepository.GetAllClaims());
    // IActionResult ist der Return-Type f�r API Methoden, hier -> Ok(), Ok (Code 200) steht f�r eine erfolgreiche Durchf�hrung


    /// <summary>
    /// Sucht Sch�rfrecht �ber eine ID
    /// </summary>
    /// <param name="id">ID des Claims auf der Datenbank</param>
    /// <returns>Das �ber die ID gefundene Element</returns>
    [HttpGet("{id}")]
    public IActionResult GetClaimById(int id)
    {
        var claim = _claimRepository.GetClaimById(id);
        return claim != null ? Ok(claim) : NotFound($"Kein Sch�rfrecht mit der ID {id} gefunden.");
    }


    /// <summary>
    /// Sucht Sch�rfrecht �ber ClaimNumber
    /// </summary>
    /// <param name="claimNumber">ClaimNumber des Sch�rfrechts</param>
    /// <returns>Das �ber die ClaimNumber gefundene Element</returns>
    [HttpGet("search")]
    public IActionResult GetByClaimNumber([FromQuery] string claimNumber)
    {
        var claim = _claimRepository.GetByClaimNumber(claimNumber);
        return claim != null ? Ok(claim) : NotFound($"Kein Sch�rfrecht mit der Nummer {claimNumber} gefunden.");
    }


    /// <summary>
    /// Erstellt ein neues Sch�rfrecht auf der Datenbank
    /// </summary>
    /// <param name="claim">Sch�rfrecht-Objekt</param>
    /// <returns>Best�tigung der Erstellung</returns>
    [HttpPost]
    public IActionResult CreateClaim(Claim claim)
    {
        if (string.IsNullOrWhiteSpace(claim.ClaimNumber) ||
            string.IsNullOrWhiteSpace(claim.OwnerName) ||
            string.IsNullOrWhiteSpace(claim.Location))
        {
            return BadRequest( new { message = "Alle Felder m�ssen bef�llt sein" });
        }

        if (claim.GoldAmount <= 0)
            return BadRequest(new { message = "Goldmenge muss �ber 0 sein." });

        if (claim.DateClaimed == default(DateTime) || claim.DateClaimed > DateTime.Now)
            return BadRequest(new { message = "Datum darf weder leer sein, noch in der Zukunft liegen." });

        int newClaimId = _claimRepository.CreateClaim(claim);
        if (newClaimId > 0)
            return CreatedAtAction(nameof(GetClaimById), new { id = newClaimId }, claim);

        return StatusCode(500, "Fehler beim ERstellen des Sch�rfrechts.");
    }


    /// <summary>
    /// Aktualisiert die Daten eines Sch�rfrechts
    /// </summary>
    /// <param name="id">ID auf der Datenbank</param>
    /// <param name="claim">Sch�rfrecht-Objekt</param>
    /// <returns>Best�tigung der erfolgreichen �nderung</returns>
    [HttpPut("{id}")]
    public IActionResult UpdateClaim(int id, Claim claim)
    {
        if (!_claimRepository.UpdateClaim(id, claim))
            return NotFound($"Sch�rfrecht mit der ID {id} nicht gefunden. Aktualisierung nicht durchgef�hrt.");

        return Ok($"Sch�rfrecht mit der ID {id} wurde erfolgreich aktualisiert");
    }


    /// <summary>
    /// L�scht einen Sch�rfrecht-Datensatz �ber ID
    /// </summary>
    /// <param name="id">ID des Sch�rfrechts auf der Datenbank</param>
    /// <returns>Best�tigung der erfolgreichen L�schung</returns>
    [HttpDelete("{id}")]
    public IActionResult DeleteClaim(int id)
    {
        if (!_claimRepository.DeleteClaim(id))
            return NotFound($"Das zu l�schende Sch�rfrecht mit der ID {id} konnte nicht gefunden werden.");
        return Ok($"Sch�rfrecht mit der ID {id} wurde erfolgreich gel�scht.");
    }
}
