using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetJourneyTutorApi.Data;

namespace PetJourneyTutorApi.Controllers;

[ApiController]
[Route("api/racas")]
public class BreedsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BreedsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista todas as raças.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
                var breeds = await _context.Breeds
            .OrderBy(r => r.NmRaca)
            .ToListAsync();

        return Ok(breeds);
    }

    /// <summary>
    /// Busca uma raça pelo identificador.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var breed = await _context.Breeds.FindAsync(id);

        if (breed == null)
            return NotFound();

        return Ok(breed);
    }
}