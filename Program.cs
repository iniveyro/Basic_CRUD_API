using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<Api.Models.TicketsDb>(opt => opt.UseInMemoryDatabase("TicketsList"));
builder.Services.AddDbContext<Api.Models.TicketsDb>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TicketAPI";
    config.Title = "TicketAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TicketAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
//}
//agregar etiquetas sin que el proyecto sea mvc
app.TicketItemsEnpoint();
app.PcItemsEndpoint();
app.AreaItemsEnpoint();
app.Run();