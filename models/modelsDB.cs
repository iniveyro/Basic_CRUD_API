using Microsoft.EntityFrameworkCore;

class TicketsDb : DbContext
{
    public TicketsDb(DbContextOptions<TicketsDb> options)
        : base(options) { }
    public DbSet<Ticket> Tickets => Set<Ticket>();
}