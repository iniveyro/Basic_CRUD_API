using Microsoft.EntityFrameworkCore;

public static class PcEndpoints
{
    public static void PcItemsEndpoint(this WebApplication app)
    {
        var PcItems = app.MapGroup("/Pcitems");

        PcItems.MapGet("/",GetAllPc);
        PcItems.MapGet("/{id}",GetPc);
        PcItems.MapPost("/",CreatePc);
        PcItems.MapDelete("/{id}",DeletePc);
        PcItems.MapPut("/{id}",UpdatePc);

    }
    static async Task<IResult> GetAllPc(Api.Models.TicketsDb db)
    {
        return TypedResults.Ok(await db.Pcs.ToArrayAsync());    }
    static async Task<IResult> GetPc(int id, Api.Models.TicketsDb db)
    {
        return await db.Pcs.FindAsync(id)
            is Api.Models.Pc Pc
            ? TypedResults.Ok(Pc)
            : TypedResults.NotFound();
    }
    static async Task<IResult> CreatePc(Api.Models.Pc Pc, Api.Models.TicketsDb db)
    {
        db.Pcs.Add(Pc);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/Pcitems/{Pc.NumInv}",Pc);
    }
    static async Task<IResult> UpdatePc(int id, Api.Models.Pc inputPc, Api.Models.TicketsDb db)
    {
        var Pc = await db.Pcs.FindAsync(id);

            if (Pc is null) return TypedResults.NotFound();

            Pc.NumInv = inputPc.NumInv;

            await db.SaveChangesAsync();
            return TypedResults.NoContent();
    }
    static async Task<IResult> DeletePc(int id, Api.Models.TicketsDb db)
    {

        if (await db.Pcs.FindAsync(id) is Api.Models.Pc Pc)
        {
            db.Pcs.Remove(Pc);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}