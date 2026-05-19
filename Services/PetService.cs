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
        var petCount = await _context.Pets.CountAsync(p => p.IdPet == petId);

        if (petCount == 0)
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

        var petCount = await _context.Pets.CountAsync(p => p.IdPet == petId);

        if (petCount == 0)
            return (false, "Pet não encontrado.", null);

        var timeline = await _context.Reminders
            .Where(r => r.IdPet == petId &&
                        r.DtLembrete.Month == mes &&
                        r.DtLembrete.Year == ano)
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
        var tutorCount = await _context.Tutors.CountAsync(t => t.IdTutor == pet.IdTutor);

        if (tutorCount == 0)
            return (false, "O tutor informado não existe.", null);

        var speciesCount = await _context.Species.CountAsync(e => e.IdEspecie == pet.IdEspecie);

        if (speciesCount == 0)
            return (false, "A espécie informada não existe.", null);

        if (pet.IdRaca != null)
        {
            var breedCount = await _context.Breeds
                .CountAsync(r => r.IdRaca == pet.IdRaca && r.IdEspecie == pet.IdEspecie);

            if (breedCount == 0)
                return (false, "A raça informada não existe ou não pertence à espécie escolhida.", null);
        }

        if (pet.IdClinica != null)
        {
            var clinicCount = await _context.Clinics.CountAsync(c => c.IdClinica == pet.IdClinica);

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

        var tutorCount = await _context.Tutors.CountAsync(t => t.IdTutor == pet.IdTutor);

        if (tutorCount == 0)
            return (false, "O tutor informado não existe.");

        var speciesCount = await _context.Species.CountAsync(e => e.IdEspecie == pet.IdEspecie);

        if (speciesCount == 0)
            return (false, "A espécie informada não existe.");

        if (pet.IdRaca != null)
        {
            var breedCount = await _context.Breeds
                .CountAsync(r => r.IdRaca == pet.IdRaca && r.IdEspecie == pet.IdEspecie);

            if (breedCount == 0)
                return (false, "A raça informada não existe ou não pertence à espécie escolhida.");
        }

        if (pet.IdClinica != null)
        {
            var clinicCount = await _context.Clinics.CountAsync(c => c.IdClinica == pet.IdClinica);

            if (clinicCount == 0)
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

        var reminderCount = await _context.Reminders.CountAsync(r => r.IdPet == id);

        if (reminderCount > 0)
            return (false, "Não é possível remover um pet que possui lembretes cadastrados.");

        _context.Pets.Remove(pet);
        await _context.SaveChangesAsync();

        return (true, null);
    }
}