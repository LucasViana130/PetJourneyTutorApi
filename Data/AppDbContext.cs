using Microsoft.EntityFrameworkCore;
using PetJourneyTutorApi.Models;

namespace PetJourneyTutorApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Tutor> Tutors { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Reminder> Reminders { get; set; }
    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Species> Species { get; set; }
    public DbSet<Breed> Breeds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tutor>().ToTable("TBTUTOR");
        modelBuilder.Entity<Pet>().ToTable("TBPET");
        modelBuilder.Entity<Reminder>().ToTable("TBLEMBRETE");
        modelBuilder.Entity<Clinic>().ToTable("TBCLINICA");
        modelBuilder.Entity<Species>().ToTable("TBESPECIE");
        modelBuilder.Entity<Breed>().ToTable("TBRACA");

        modelBuilder.Entity<Tutor>()
            .HasIndex(t => t.DsEmail)
            .IsUnique();

        modelBuilder.Entity<Reminder>()
            .Property(r => r.DsStatus)
            .HasDefaultValue("PENDENTE");

        modelBuilder.Entity<Tutor>()
            .Property(t => t.DtCadastro)
            .HasDefaultValueSql("SYSDATE");
    }
}