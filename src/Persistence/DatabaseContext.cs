using Microsoft.EntityFrameworkCore;
using src.Models;

namespace src.Persistence;

public class DatabaseContext : DbContext
{
    public DbSet<Person> Pessoas { get; set; }
    public DbSet<Contract> Contratos { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Person>(t =>
        {
            t.HasKey(e => e.Id);
            t.HasMany(e => e.Contratos).WithOne().HasForeignKey(c => c.PessoaId);
        });

        builder.Entity<Contract>(t =>
        {
            t.HasKey(e => e.Id);
        });
    }
}