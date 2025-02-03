using Microsoft.EntityFrameworkCore;

public static class AreaEndpoints
{
    public static void AreaItemsEnpoint(this WebApplication app)
    {
    
        var areaitems = app.MapGroup("/areaitems");

        areaitems.MapGet("/", GetAllAreas);
        areaitems.MapGet("/{id}", GetArea);
        areaitems.MapPost("/", CreateArea);
        areaitems.MapPut("/{id}", UpdateArea);
        areaitems.MapDelete("/{id}", DeleteArea);
        
        static async Task<IResult> GetAllAreas(TicketsDb db)
        {
            return TypedResults.Ok(await db.Areas.ToArrayAsync());
        }
        static async Task<IResult> GetArea(int id, TicketsDb db)
        {
            return await db.Areas.FindAsync(id)
                is Area area
                ? TypedResults.Ok(area)
                : TypedResults.NotFound();
        }
        static async Task<IResult> CreateArea(Area area,TicketsDb db)
        {
            db.Areas.Add(area);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/areaitems/{area.Name}",area);
        }
        static async Task<IResult> UpdateArea (string name,  Area inputArea, TicketsDb db)
        {
            var area = await db.Areas.FindAsync(name);

            if (area is null) return TypedResults.NotFound();

            area.Name = inputArea.Name;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }
        static async Task<IResult> DeleteArea (string name, TicketsDb db)
        {
            if (await db.Areas.FindAsync(name) is Area area)
            {
                db.Areas.Remove(area);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }
            return TypedResults.NotFound();
        }
    }
} 