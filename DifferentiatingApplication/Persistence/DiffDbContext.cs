global using Microsoft.EntityFrameworkCore;
using DifferentiatingApplication.Domain;

namespace DifferentiatingApplication.Persistence;

public class DiffDbContext : DbContext
{
    public DiffDbContext(DbContextOptions<DiffDbContext> options) : base(options)
    {
    }

    public DbSet<DataRecord> DataRecords { get; set; }
    /// <summary>
    /// Method to configure the data model
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DataRecord>()
            .HasKey(d => new { d.Id, d.Side });
    }
}
