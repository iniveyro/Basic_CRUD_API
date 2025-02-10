using Microsoft.EntityFrameworkCore;

namespace Api.Models;
class TicketsDb : DbContext
{
    public TicketsDb(DbContextOptions<TicketsDb> options)
        : base(options) { }
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Pc> Pcs => Set<Pc>();
    public DbSet<Area> Areas => Set<Area>();
}