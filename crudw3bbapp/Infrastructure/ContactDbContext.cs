using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Service.Common.Interfaces;

namespace Infrastructure;

public class ContactDbContext : DbContext, IContactDbContext
{
    public const string MIGRATIONS_HISTORY_TABLE_NAME = "__EFMigrationsHistory";
    public const string SCHEMA_NAME = "dbo";

    public ContactDbContext()
    {
    }

    public ContactDbContext(DbContextOptions<ContactDbContext> options)
    : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(builder =>
            {
                builder.MigrationsHistoryTable(MIGRATIONS_HISTORY_TABLE_NAME, SCHEMA_NAME);
            });
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Contact>(entity =>
        {

            entity.Property(e => e.Id)
                   .ValueGeneratedNever();

            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName)
                   .IsRequired();

            entity.Property(e => e.LastName);
            entity.Property(e => e.Email);
            entity.Property(e => e.PhoneNumber);
            entity.Property(e => e.Company);
        });

        base.OnModelCreating(modelBuilder);
    }
}
