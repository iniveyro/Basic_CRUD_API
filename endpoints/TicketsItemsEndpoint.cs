using Microsoft.EntityFrameworkCore;

public static class TicketEnpoint
{
    public static void TicketItemsEnpoint(this WebApplication app)
    {
        var ticketitems = app.MapGroup("/ticketitems");

        ticketitems.MapGet("/", GetAllTickets);
        ticketitems.MapGet("/{id}", GetTicket);
        ticketitems.MapGet("/status", GetStatusTickets);
        ticketitems.MapPost("/", CreateTicket);
        ticketitems.MapPut("/{id}", UpdateTicket);
        ticketitems.MapDelete("/{id}", DeleteTicket);
        
        static async Task<IResult> GetAllTickets(Api.Models.TicketsDb db)
        {
            return TypedResults.Ok(await db.Tickets.ToArrayAsync());
        }

        static async Task<IResult> GetStatusTickets(Api.Models.TicketsDb db)
        {
            return TypedResults.Ok(await db.Tickets.Where(t=>t.Status).ToListAsync());
        }

        static async Task<IResult> GetTicket(int id, Api.Models.TicketsDb db)
        {
            return await db.Tickets.FindAsync(id)
                is Api.Models.Ticket ticket
                ? TypedResults.Ok(ticket)
                : TypedResults.NotFound();
        }

        static async Task<IResult> CreateTicket(Api.Models.Ticket ticket,Api.Models.TicketsDb db)
        {
            db.Tickets.Add(ticket);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/ticketitems/{ticket.NumId}",ticket);
        }

        static async Task<IResult> UpdateTicket (int num,  Api.Models.Ticket inputTicket, Api.Models.TicketsDb db)
        {
            var ticket = await db.Tickets.FindAsync(num);

            if (ticket is null) return TypedResults.NotFound();

            ticket.NumId = inputTicket.NumId;
            ticket.Status = inputTicket.Status;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        static async Task<IResult> DeleteTicket (int num, Api.Models.TicketsDb db)
        {
            if (await db.Tickets.FindAsync(num) is Api.Models.Ticket ticket)
            {
                db.Tickets.Remove(ticket);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }
} 