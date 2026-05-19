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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var species = await _context.Species
            .OrderBy(e => e.NmEspecie)
            .ToListAsync();

        return Ok(species);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var species = await _context.Species.FindAsync(id);

        if (species == null)
            return NotFound();

        return Ok(species);
    }

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