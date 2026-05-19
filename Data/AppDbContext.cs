using Microsoft.EntityFrameworkCore;
using PetJourneyTutorApi.Models;

namespace PetJourneyTutorApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Tutor> Tutors => Set<Tutor>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<Reminder> Reminders => Set<Reminder>();
    public DbSet<Clinic> Clinics => Set<Clinic>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(t => t.IdTutor);
            entity.HasIndex(t => t.DsEmail).IsUnique();
            entity.Property(t => t.DtCadastro).HasDefaultValueSql("SYSDATE");
            entity.Property(t => t.DsPlano).HasDefaultValue("FREE");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(p => p.IdPet);
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(r => r.IdLembrete);
            entity.Property(r => r.DsStatus).HasDefaultValue("PENDENTE");
        });

        modelBuilder.Entity<Clinic>(entity =>
        {
            entity.HasKey(c => c.IdClinica);
        });
    }
}
