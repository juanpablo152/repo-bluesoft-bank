using back_bluesoft_bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace back_bluesoft_bank.Infrastructure.Data;

public class BlueSoftBankDbContext : DbContext
{
    public BlueSoftBankDbContext(DbContextOptions<BlueSoftBankDbContext> options) : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlueSoftBankDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
