using Microsoft.AspNetCore.Mvc;
using PetJourneyTutorApi.Models;
using PetJourneyTutorApi.Services;

namespace PetJourneyTutorApi.Controllers;

[ApiController]
[Route("api/clinicas")]
public class ClinicsController : ControllerBase
{
    private readonly ClinicService _clinicService;

    public ClinicsController(ClinicService clinicService)
    {
        _clinicService = clinicService;
    }

    /// <summary>
    /// Lista todas as clínicas.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Clinic>>> GetClinics()
    {
        var clinics = await _clinicService.GetAllAsync();
        return Ok(clinics);
    }

    /// <summary>
    /// Busca uma clínica pelo identificador.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Clinic>> GetClinicById(int id)
    {
        var clinic = await _clinicService.GetByIdAsync(id);

        if (clinic == null)
            return NotFound("Clínica não encontrada.");

        return Ok(clinic);
    }
}
