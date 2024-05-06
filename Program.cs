using chairs_dotnet7_api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("chairlist"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

var chairs = app.MapGroup("api/chair");

//TODO: ASIGNACION DE RUTAS A LOS ENDPOINTS
chairs.MapGet("/", GetChairs);
chairs.MapGet("/", GetAllChairs);
chairs.MapPost("/", CreateChair);
chairs.MapGet("/{name}", GetChairByName);
chairs.MapDelete("/{id}", DeleteChair);

app.Run();

//TODO: ENDPOINTS SOLICITADOS
static IResult GetChairs(DataContext db)
{
    return TypedResults.Ok();
}

static async Task<IResult> GetAllChairs(DataContext db){
    return TypedResults.Ok(await db.Chairs.ToArrayAsync());
}
static async Task<IResult> CreateChair(Chair chair,DataContext db){
    db.Chairs.Add(chair);
    await db.SaveChangesAsync();
    return TypedResults.Created($"api/chair/{Chair.Id}", chair);
}
static async Task<IResult> GetChairByName(string name,DataContext db){
    return await db.Chairs.FindAsync(name)
    is Chair chair
    ? TypedResults.Ok(chair)
    : TypedResults.NotFound();
}
static async Task<IResult> DeleteChair(int id, DataContext db){
    if (await db.Chairs.FindAsync(id) is Chair chair){
        db.Chairs.Remove(chair);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NotFound();
}
