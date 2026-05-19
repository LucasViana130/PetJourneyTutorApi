using Microsoft.EntityFrameworkCore;
using PetJourneyTutorApi.Data;
using PetJourneyTutorApi.Models;

namespace PetJourneyTutorApi.Services;

public class TutorService
{
    private readonly AppDbContext _context;

    public TutorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tutor>> GetAllAsync()
    {
        return await _context.Tutors
            .OrderBy(t => t.IdTutor)
            .ToListAsync();
    }

    public async Task<Tutor?> GetByIdAsync(int id)
    {
        return await _context.Tutors.FindAsync(id);
    }

    public async Task<List<Pet>?> GetPetsByTutorAsync(int id)
    {
        var tutorExists = await _context.Tutors.AnyAsync(t => t.IdTutor == id);

        if (!tutorExists)
            return null;

        return await _context.Pets
            .Where(p => p.IdTutor == id)
            .OrderBy(p => p.NmPet)
            .ToListAsync();
    }

    public async Task<List<Clinic>?> GetTutorClinicsAsync(int tutorId)
    {
        var tutorExists = await _context.Tutors.AnyAsync(t => t.IdTutor == tutorId);

        if (!tutorExists)
            return null;

        var clinicIds = await _context.Pets
            .Where(p => p.IdTutor == tutorId && p.IdClinica != null)
            .Select(p => p.IdClinica!.Value)
            .Distinct()
            .ToListAsync();

        return await _context.Clinics
            .Where(c => clinicIds.Contains(c.IdClinica))
            .OrderBy(c => c.NmClinica)
            .ToListAsync();
    }

    public async Task<(bool Success, string? Error, Tutor? Tutor)> CreateAsync(Tutor tutor)
    {
        var emailAlreadyExists = await _context.Tutors.AnyAsync(t => t.DsEmail == tutor.DsEmail);

        if (emailAlreadyExists)
            return (false, "Já existe um tutor cadastrado com este e-mail.", null);

        tutor.DtCadastro = DateTime.Now;

        if (string.IsNullOrWhiteSpace(tutor.DsPlano))
            tutor.DsPlano = "FREE";

        _context.Tutors.Add(tutor);
        await _context.SaveChangesAsync();

        return (true, null, tutor);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(int id, Tutor tutor)
    {
        if (id != tutor.IdTutor)
            return (false, "O id da rota é diferente do id enviado no corpo da requisição.");

        var currentTutor = await _context.Tutors.FindAsync(id);

        if (currentTutor == null)
            return (false, "Tutor não encontrado.");

        var emailAlreadyExists = await _context.Tutors
            .AnyAsync(t => t.DsEmail == tutor.DsEmail && t.IdTutor != id);

        if (emailAlreadyExists)
            return (false, "Já existe outro tutor cadastrado com este e-mail.");

        currentTutor.NmTutor = tutor.NmTutor;
        currentTutor.DsEmail = tutor.DsEmail;
        currentTutor.NrTelefone = tutor.NrTelefone;
        currentTutor.DsPlano = string.IsNullOrWhiteSpace(tutor.DsPlano) ? currentTutor.DsPlano : tutor.DsPlano;

        await _context.SaveChangesAsync();

        return (true, null);
    }

    // Afiliar tutor à clínica sem alterar a tabela TBTUTOR.
    // O vínculo fica salvo nos pets do tutor, usando a coluna TBPET.IDCLINICA já existente no banco.
    public async Task<(bool Success, string? Error)> LinkToClinicAsync(int tutorId, int clinicaId)
    {
        var tutorExists = await _context.Tutors.AnyAsync(t => t.IdTutor == tutorId);

        if (!tutorExists)
            return (false, "Tutor não encontrado.");

        var clinicExists = await _context.Clinics.AnyAsync(c => c.IdClinica == clinicaId);

        if (!clinicExists)
            return (false, "Clínica não encontrada.");

        var pets = await _context.Pets
            .Where(p => p.IdTutor == tutorId)
            .ToListAsync();

        if (!pets.Any())
            return (false, "Cadastre pelo menos um pet antes de se afiliar a uma clínica.");

        foreach (var pet in pets)
            pet.IdClinica = clinicaId;

        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> UnlinkFromClinicAsync(int tutorId)
    {
        var tutorExists = await _context.Tutors.AnyAsync(t => t.IdTutor == tutorId);

        if (!tutorExists)
            return (false, "Tutor não encontrado.");

        var pets = await _context.Pets
            .Where(p => p.IdTutor == tutorId)
            .ToListAsync();

        foreach (var pet in pets)
            pet.IdClinica = null;

        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var tutor = await _context.Tutors.FindAsync(id);

        if (tutor == null)
            return (false, "Tutor não encontrado.");

        var hasPets = await _context.Pets.AnyAsync(p => p.IdTutor == id);

        if (hasPets)
            return (false, "Não é possível remover um tutor que possui pets cadastrados.");

        _context.Tutors.Remove(tutor);
        await _context.SaveChangesAsync();

        return (true, null);
    }
}
