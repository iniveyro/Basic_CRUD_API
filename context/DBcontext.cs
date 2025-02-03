using Microsoft.EntityFrameworkCore;

namespace Api.Models;
class TicketsDb : DbContext
{
    public TicketsDb(DbContextOptions<TicketsDb> options)
        : base(options) { }
    public DbSet<Api.Models.Ticket> Tickets => Set<Ticket>();
    public DbSet<Api.Models.PC> PCs => Set<PC>();
    public DbSet<Api.Models.Area> Areas => Set<Area>();
}