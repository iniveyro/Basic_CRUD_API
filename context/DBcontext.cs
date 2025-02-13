using Microsoft.EntityFrameworkCore;

namespace Api.Models;
class TicketsDb : DbContext
{
    public TicketsDb(DbContextOptions<TicketsDb> options)
        : base(options) { }
    public DbSet<Area> Areas { get; set; } 
    public DbSet<Pc> Pcs { get; set; } 
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}