using Microsoft.EntityFrameworkCore;

class TicketsDb : DbContext
{
    public TicketsDb(DbContextOptions<TicketsDb> options)
        : base(options) { }
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<PC> PC => Set<PC>();
    public DbSet<Area> Area => Set<Area>();
}