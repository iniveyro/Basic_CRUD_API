using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TicketsDb>(opt => opt.UseInMemoryDatabase("TickersList"));
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

ticketitems.MapGet("/", async (TicketsDb db) =>
    await db.Tickets.ToListAsync());

ticketitems.MapGet("/status", async (TicketsDb db) =>
    await db.Tickets.Where(t => t.Status).ToListAsync());

ticketitems.MapGet("/{id}", async (int id, TicketsDb db) =>
    await db.Tickets.FindAsync(id)
        is Ticket ticket
            ? Results.Ok(ticket)
            : Results.NotFound());

ticketitems.MapPost("/", async (Ticket ticket, TicketsDb db) =>
{
    db.Tickets.Add(ticket);
    await db.SaveChangesAsync();

    return Results.Created($"/ticketitems/{ticket.NumId}", ticket);
});

ticketitems.MapPut("/{id}", async (int id, Ticket inputTicket, TicketsDb db) =>
{
    var ticket = await db.Tickets.FindAsync(id);

    if (ticket is null) return Results.NotFound();

    ticket.Description = inputTicket.Description;
    ticket.Status = inputTicket.Status;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/{id}", async (int id, TicketsDb db) =>
{
    if (await db.Tickets.FindAsync(id) is Ticket ticket)
    {
        db.Tickets.Remove(ticket);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();