using LeadsManagement.Domain.Entities;
using LeadsManagement.Infra.Contexts.Mappings;
using Microsoft.EntityFrameworkCore;

namespace LeadsManagement.Infra.Contexts;

public class DataContext : DbContext
{
    protected DataContext() : base() {}

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<Lead> Leads { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LeadMap());
    }
}