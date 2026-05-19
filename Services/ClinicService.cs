using Microsoft.EntityFrameworkCore;
using PetJourneyTutorApi.Data;
using PetJourneyTutorApi.Models;

namespace PetJourneyTutorApi.Services;

public class ClinicService
{
    private readonly AppDbContext _context;

    public ClinicService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Clinic>> GetAllAsync()
    {
        return await _context.Clinics
            .OrderBy(c => c.NmClinica)
            .ToListAsync();
    }

    public async Task<Clinic?> GetByIdAsync(int id)
    {
        return await _context.Clinics.FindAsync(id);
    }
}
