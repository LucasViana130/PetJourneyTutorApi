using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetJourneyTutorApi.Data;

namespace PetJourneyTutorApi.Controllers;

[ApiController]
[Route("api/especies")]
public class SpeciesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SpeciesController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista todas as espécies pré-cadastradas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var species = await _context.Species
            .OrderBy(e => e.NmEspecie)
            .ToListAsync();

        return Ok(species);
    }

    /// <summary>
    /// Busca uma espécie pelo identificador.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var species = await _context.Species.FindAsync(id);

        if (species == null)
            return NotFound();

        return Ok(species);
    }

    /// <summary>
    /// Lista as raças vinculadas a uma espécie.
    /// </summary>
    [HttpGet("{id}/racas")]
    public async Task<IActionResult> GetBreedsBySpecies(int id)
    {
        var speciesCount = await _context.Species.CountAsync(e => e.IdEspecie == id);

        if (speciesCount == 0)
            return NotFound("Espécie não encontrada.");

        var breeds = await _context.Breeds
            .Where(r => r.IdEspecie == id)
            .OrderBy(r => r.NmRaca)
            .ToListAsync();

        return Ok(breeds);
    }
}