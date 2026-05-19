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
        var petCount = await _context.Pets.CountAsync(p => p.IdPet == reminder.IdPet);

        if (petCount == 0)
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

        var currentReminder = await _context.Reminders.FindAsync(id);

        if (currentReminder == null)
            return (false, "Lembrete não encontrado.");

        var petCount = await _context.Pets.CountAsync(p => p.IdPet == reminder.IdPet);

        if (petCount == 0)
            return (false, "O pet informado não existe.");

        currentReminder.IdPet = reminder.IdPet;
        currentReminder.DsTipo = reminder.DsTipo;
        currentReminder.DsDescricao = reminder.DsDescricao;
        currentReminder.DtLembrete = reminder.DtLembrete;
        currentReminder.DtNotificado = reminder.DtNotificado;
        currentReminder.DsStatus = reminder.DsStatus;

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