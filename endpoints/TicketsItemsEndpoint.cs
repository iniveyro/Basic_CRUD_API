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

        static async Task<IResult> CreateTicket(Api.Models.Ticket ticket, Api.Models.TicketsDb db)
        {
            // Busca el área existente por nombre
            var areaExistente = await db.Areas.FindAsync(ticket.Area.Name);

            if (areaExistente != null)
            {
                // Si existe, asigna los valores del área existente al ticket
                ticket.Area = areaExistente;
            }
            else
            {
                // Si no existe, crea un nuevo área
                var nuevaArea = new Api.Models.Area { Name = ticket.Area.Name, Location = ticket.Area.Location };
                db.Areas.Add(nuevaArea);
                await db.SaveChangesAsync();
                ticket.Area = nuevaArea;
            }

            // Repite el proceso para PC
            var pcExistente = await db.Pcs.FindAsync(ticket.Pc.NumInv);

            if (pcExistente != null)
            {
                // Si existe, asigna los valores del área existente al ticket
                ticket.Pc = pcExistente;
            }
            else
            {
                // Si no existe, crea un nuevo pc
                var nuevoPc = new Api.Models.Pc { NumInv = ticket.Pc.NumInv, NumSer = ticket.Pc.NumSer, Description = ticket.Pc.Description};
                db.Pcs.Add(nuevoPc);
                await db.SaveChangesAsync();
                ticket.Pc = nuevoPc;
            }
            
            // Agrega el nuevo ticket
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