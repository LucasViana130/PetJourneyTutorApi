using Microsoft.EntityFrameworkCore;
using PetJourneyTutorApi.Data;
using PetJourneyTutorApi.Models;

namespace PetJourneyTutorApi.Services;

public class PetService
{
    private readonly AppDbContext _context;

    public PetService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pet>> GetAllAsync()
    {
        return await _context.Pets
            .OrderBy(p => p.IdPet)
            .ToListAsync();
    }

    public async Task<Pet?> GetByIdAsync(int id)
    {
        return await _context.Pets.FindAsync(id);
    }

    public async Task<List<Reminder>?> GetRemindersByPetAsync(int petId)
    {
        var petExists = await _context.Pets.AnyAsync(p => p.IdPet == petId);

        if (!petExists)
            return null;

        return await _context.Reminders
            .Where(r => r.IdPet == petId)
            .OrderBy(r => r.DtLembrete)
            .ToListAsync();
    }

    public async Task<(bool Success, string? Error, List<TimelineItem>? Timeline)> GetTimelineAsync(int petId, int mes, int ano)
    {
        if (mes < 1 || mes > 12)
            return (false, "O mês deve estar entre 1 e 12.", null);

        if (ano < 2000)
            return (false, "Informe um ano válido.", null);

        var petExists = await _context.Pets.AnyAsync(p => p.IdPet == petId);

        if (!petExists)
            return (false, "Pet não encontrado.", null);

        var timeline = await _context.Reminders
            .Where(r => r.IdPet == petId && r.DtLembrete.Month == mes && r.DtLembrete.Year == ano)
            .OrderBy(r => r.DtLembrete)
            .Select(r => new TimelineItem
            {
                Tipo = r.DsTipo,
                Descricao = r.DsDescricao,
                Data = r.DtLembrete,
                Status = r.DsStatus
            })
            .ToListAsync();

        return (true, null, timeline);
    }

    public async Task<(bool Success, string? Error, Pet? Pet)> CreateAsync(Pet pet)
    {
        var tutorCount = await _context.Tutors
            .CountAsync(t => t.IdTutor == pet.IdTutor);

        if (tutorCount == 0)
            return (false, "O tutor informado não existe.", null);

        if (pet.IdClinica != null)
        {
            var clinicCount = await _context.Clinics
                .CountAsync(c => c.IdClinica == pet.IdClinica);

            if (clinicCount == 0)
                return (false, "A clínica informada não existe.", null);
        }

        _context.Pets.Add(pet);
        await _context.SaveChangesAsync();
        
        return (true, null, pet);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(int id, Pet pet)
    {
        if (id != pet.IdPet)
            return (false, "O id da rota é diferente do id enviado no corpo da requisição.");

        var currentPet = await _context.Pets.FindAsync(id);

        if (currentPet == null)
            return (false, "Pet não encontrado.");

        var tutorExists = await _context.Tutors.AnyAsync(t => t.IdTutor == pet.IdTutor);

        if (!tutorExists)
            return (false, "O tutor informado não existe.");

        if (pet.IdClinica != null)
        {
            var clinicExists = await _context.Clinics.AnyAsync(c => c.IdClinica == pet.IdClinica);

            if (!clinicExists)
                return (false, "A clínica informada não existe.");
        }

        currentPet.NmPet = pet.NmPet;
        currentPet.DtNascimento = pet.DtNascimento;
        currentPet.DsSexo = pet.DsSexo;
        currentPet.IdTutor = pet.IdTutor;
        currentPet.IdEspecie = pet.IdEspecie;
        currentPet.IdRaca = pet.IdRaca;
        currentPet.IdClinica = pet.IdClinica;

        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var pet = await _context.Pets.FindAsync(id);

        if (pet == null)
            return (false, "Pet não encontrado.");

        var hasReminders = await _context.Reminders.AnyAsync(r => r.IdPet == id);

        if (hasReminders)
            return (false, "Não é possível remover um pet que possui lembretes cadastrados.");

        _context.Pets.Remove(pet);
        await _context.SaveChangesAsync();

        return (true, null);
    }
}
