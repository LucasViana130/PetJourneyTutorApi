using Microsoft.AspNetCore.Mvc;
using PetJourneyTutorApi.Models;
using PetJourneyTutorApi.Services;

namespace PetJourneyTutorApi.Controllers;

[ApiController]
[Route("api/lembretes")]
public class RemindersController : ControllerBase
{
    private readonly ReminderService _reminderService;

    public RemindersController(ReminderService reminderService)
    {
        _reminderService = reminderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reminder>>> GetReminders()
    {
        var reminders = await _reminderService.GetAllAsync();
        return Ok(reminders);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Reminder>> GetReminderById(int id)
    {
        var reminder = await _reminderService.GetByIdAsync(id);

        if (reminder == null)
            return NotFound("Lembrete não encontrado.");

        return Ok(reminder);
    }

    [HttpPost]
    public async Task<ActionResult<Reminder>> CreateReminder(Reminder reminder)
    {
        var result = await _reminderService.CreateAsync(reminder);

        if (!result.Success)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetReminderById), new { id = result.Reminder!.IdLembrete }, result.Reminder);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateReminder(int id, Reminder reminder)
    {
        var result = await _reminderService.UpdateAsync(id, reminder);

        if (!result.Success)
        {
            if (result.Error == "Lembrete não encontrado.")
                return NotFound(result.Error);

            return BadRequest(result.Error);
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteReminder(int id)
    {
        var result = await _reminderService.DeleteAsync(id);

        if (!result.Success)
            return NotFound(result.Error);

        return NoContent();
    }
}
