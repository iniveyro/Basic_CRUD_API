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
    static async Task<IResult> GetAllPC(Api.Models.TicketsDb db)
    {
        return TypedResults.Ok(await db.PCs.ToArrayAsync());    }
    static async Task<IResult> GetPC(int id, Api.Models.TicketsDb db)
    {
        return await db.PCs.FindAsync(id)
            is Api.Models.PC pc
            ? TypedResults.Ok(pc)
            : TypedResults.NotFound();
    }
    static async Task<IResult> CreatePC(Api.Models.PC pc, Api.Models.TicketsDb db)
    {
        db.PCs.Add(pc);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/pcitems/{pc.NumInv}",pc);
    }
    static async Task<IResult> UpdatePC(int id, Api.Models.PC inputPc, Api.Models.TicketsDb db)
    {
        var pc = await db.PCs.FindAsync(id);

            if (pc is null) return TypedResults.NotFound();

            pc.NumInv = inputPc.NumInv;

            await db.SaveChangesAsync();
            return TypedResults.NoContent();
    }
    static async Task<IResult> DeletePC(int id, Api.Models.TicketsDb db)
    {

    if (await db.PCs.FindAsync(id) is Api.Models.PC pc)
        {
        db.PCs.Remove(pc);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}