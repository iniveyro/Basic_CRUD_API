using Microsoft.EntityFrameworkCore;

public static class PCEndpoints
{
    public static void PCItemsEndpoint(this WebApplication app)
    {
        var PCItems = app.MapGroup("/pcitems");

        PCItems.MapGet("/",GetAllPC);
        PCItems.MapGet("/{id}",GetPC);
        PCItems.MapPost("/",CreatePC);
        PCItems.MapDelete("/{id}",DeletePC);
        PCItems.MapPut("/{id}",UpdatePC);

    }
    static async Task<IResult> GetAllPC(TicketsDb db)
    {
        return TypedResults.Ok(await db.PC.ToArrayAsync());    }
    static async Task<IResult> GetPC(int id, TicketsDb db)
    {
        return await db.PC.FindAsync(id)
            is PC pc
            ? TypedResults.Ok(pc)
            : TypedResults.NotFound();
    }
    static async Task<IResult> CreatePC(PC pc, TicketsDb db)
    {
        db.PC.Add(pc);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/pcitems/{pc.NumInv}",pc);
    }
    static async Task<IResult> UpdatePC(int id, PC inputPc, TicketsDb db)
    {
        var pc = await db.PC.FindAsync(id);

            if (pc is null) return TypedResults.NotFound();

            pc.NumInv = inputPc.NumInv;

            await db.SaveChangesAsync();
            return TypedResults.NoContent();
    }
    static async Task<IResult> DeletePC(int id, TicketsDb db)
    {

    if (await db.PC.FindAsync(id) is PC pc)
        {
        db.PC.Remove(pc);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}