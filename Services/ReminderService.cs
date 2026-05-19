using Microsoft.EntityFrameworkCore;
using PetJourneyTutorApi.Data;
using PetJourneyTutorApi.Models;

namespace PetJourneyTutorApi.Services;

public class ReminderService
{
    private readonly AppDbContext _context;

    public ReminderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Reminder>> GetAllAsync()
    {
        return await _context.Reminders
            .OrderBy(r => r.DtLembrete)
            .ToListAsync();
    }

    public async Task<Reminder?> GetByIdAsync(int id)
    {
        return await _context.Reminders.FindAsync(id);
    }

    public async Task<(bool Success, string? Error, Reminder? Reminder)> CreateAsync(Reminder reminder)
    {
        var petExists = await _context.Pets.AnyAsync(p => p.IdPet == reminder.IdPet);

        if (!petExists)
            return (false, "O pet informado não existe.", null);

        if (reminder.DtLembrete.Date < DateTime.Today)
            return (false, "A data do lembrete não pode ser anterior à data atual.", null);

        if (string.IsNullOrWhiteSpace(reminder.DsStatus))
            reminder.DsStatus = "PENDENTE";

        _context.Reminders.Add(reminder);
        await _context.SaveChangesAsync();

        return (true, null, reminder);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(int id, Reminder reminder)
    {
        if (id != reminder.IdLembrete)
            return (false, "O id da rota é diferente do id enviado no corpo da requisição.");

        var exists = await _context.Reminders.AnyAsync(r => r.IdLembrete == id);

        if (!exists)
            return (false, "Lembrete não encontrado.");

        var petExists = await _context.Pets.AnyAsync(p => p.IdPet == reminder.IdPet);

        if (!petExists)
            return (false, "O pet informado não existe.");

        _context.Entry(reminder).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var reminder = await _context.Reminders.FindAsync(id);

        if (reminder == null)
            return (false, "Lembrete não encontrado.");

        _context.Reminders.Remove(reminder);
        await _context.SaveChangesAsync();

        return (true, null);
    }
}
