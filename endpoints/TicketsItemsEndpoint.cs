using Api.Models;
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
            await db.SaveChangesAsync();
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

        static async Task<IResult> CreateTicket(bool Status, string Description, PriorityLevel Priority, string Area, int PC, Api.Models.Ticket ticket, Api.Models.TicketsDb db)
        {
            // Busca el área existente por nombre
            var areaExistente = await db.Areas.FindAsync(ticket.Area);

            if (areaExistente != null)
            {
                // Si existe, asigna los valores del área existente al ticket
                ticket.Area = areaExistente.Name;
            }
            else
            {
                // Si no existe, crea un nuevo área
                var nuevaArea = new Api.Models.Area { Name = Area};
                db.Areas.Add(nuevaArea);
                ticket.Area = nuevaArea.Name;
                await db.SaveChangesAsync();
            }

            // Repite el proceso para PC
            var pcExistente = await db.Pcs.FindAsync(ticket.Pc);

            if (pcExistente != null)
            {
                // Si existe, asigna los valores del área existente al ticket
                ticket.Pc = pcExistente.NumInv;
            }
            else
            {
                // Si no existe, crea un nuevo pc
                var nuevoPc = new Api.Models.Pc { NumInv = PC};
                db.Pcs.Add(nuevoPc);
                ticket.Pc = nuevoPc.NumInv;
                await db.SaveChangesAsync();
            }
            
            // Agrega el nuevo ticket
            
            ticket.Status = Status;
            ticket.Description = Description;
            ticket.Priority = Priority;
            ticket.Area = Area;
            ticket.Pc = PC;
            
            db.Tickets.Add(ticket);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/ticketitems/{ticket.NumId}",ticket);
        }
        
        static async Task<IResult> UpdateTicket (int id,  Api.Models.Ticket inputTicket, Api.Models.TicketsDb db)
        {
            var ticket = await db.Tickets.FindAsync(id);

            if (ticket is null) return TypedResults.NotFound();

            ticket.NumId = inputTicket.NumId;
            ticket.Status = inputTicket.Status;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        static async Task<IResult> DeleteTicket (int id, Api.Models.TicketsDb db)
        {
            if (id <= 0)
            {
                return TypedResults.BadRequest("El número de ticket debe ser un valor positivo.");
            }
            try
            {
                if (await db.Tickets.FindAsync(id) is Api.Models.Ticket ticket)
                {
                    db.Tickets.Remove(ticket);
                    await db.SaveChangesAsync();
                    return TypedResults.NoContent();
                }
                return TypedResults.NotFound();
            }
            catch (Exception)
            {
                // Log the exception (ex) here
                return TypedResults.Problem("Ocurrió un error al procesar la solicitud.");
            }
        }
    }
} 