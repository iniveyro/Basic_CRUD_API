using Microsoft.EntityFrameworkCore;

public static class AreaEndpoints
{
    public static void AreaItemsEnpoint(this WebApplication app)
    {
    
        var areaitems = app.MapGroup("/areaitems");

        areaitems.MapGet("/", GetAllAreas);
        areaitems.MapGet("/{name}", GetArea);
        areaitems.MapPost("/", CreateArea);
        areaitems.MapPut("/{name}", UpdateArea);
        areaitems.MapDelete("/{name}", DeleteArea);
        
        static async Task<IResult> GetAllAreas(Api.Models.TicketsDb db)
        {
            return TypedResults.Ok(await db.Areas.ToArrayAsync());
        }
        static async Task<IResult> GetArea(int name, Api.Models.TicketsDb db)
        {
            return await db.Areas.FindAsync(name)
                is Api.Models.Area area
                ? TypedResults.Ok(area)
                : TypedResults.NotFound();
        }
        static async Task<IResult> CreateArea(Api.Models.Area area,Api.Models.TicketsDb db)
        {
            db.Areas.Add(area);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/areaitems/{area.Name}",area);
        }
        static async Task<IResult> UpdateArea (string name,  Api.Models.Area inputArea, Api.Models.TicketsDb db)
        {
            var area = await db.Areas.FindAsync(name);

            if (area is null) return TypedResults.NotFound();

            area.Name = inputArea.Name;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }
        static async Task<IResult> DeleteArea (string name, Api.Models.TicketsDb db)
        {
            if (await db.Areas.FindAsync(name) is Api.Models.Area area)
            {
                db.Areas.Remove(area);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }
            return TypedResults.NotFound();
        }
    }
} 