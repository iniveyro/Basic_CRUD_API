using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TicketsDb>(opt => opt.UseInMemoryDatabase("TicketsList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TicketAPI";
    config.Title = "TicketAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TicketAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

var ticketitems = app.MapGroup("/ticketitems");

ticketitems.MapGet("/", GetAllTickets);
ticketitems.MapGet("/{id}", GetTicket);
ticketitems.MapGet("/status", GetStatusTickets);
ticketitems.MapPost("/", CreateTicket);
ticketitems.MapPut("/{id}", UpdateTicket);
ticketitems.MapDelete("/{id}", DeleteTicket);

app.Run();

static async Task<IResult> GetAllTickets(TicketsDb db)
{
    return TypedResults.Ok(await db.Tickets.ToArrayAsync());
}

static async Task<IResult> GetStatusTickets(TicketsDb db)
{
    return TypedResults.Ok(await db.Tickets.Where(t=>t.Status).ToListAsync());
}

static async Task<IResult> GetTicket(int id, TicketsDb db)
{
    return await db.Tickets.FindAsync(id)
        is Ticket ticket
        ? TypedResults.Ok(ticket)
        : TypedResults.NotFound();
}

static async Task<IResult> CreateTicket(Ticket ticket,TicketsDb db)
{
    db.Tickets.Add(ticket);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/ticketitems/{ticket.NumId}",ticket);
}

static async Task<IResult> UpdateTicket (int num,  Ticket inputTicket, TicketsDb db)
{
    var ticket = await db.Tickets.FindAsync(num);

    if (ticket is null) return TypedResults.NotFound();

    ticket.NumId = inputTicket.NumId;
    ticket.Status = inputTicket.Status;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTicket (int num, TicketsDb db)
{
    if (await db.Tickets.FindAsync(num) is Ticket ticket)
    {
        db.Tickets.Remove(ticket);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}