using Microsoft.EntityFrameworkCore;
using MinimalAPIPostgreSQLEntityFramework.Data;
using MinimalAPIPostgreSQLEntityFramework.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Obtenemos connectionString del archivo appsettings.json
string connectionString = builder.Configuration.GetConnectionString("PostgreSQL");
builder.Services.AddDbContext<OfficeDb>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/* ========== Codigo correspondiente a la Minimal API ========== */

// INSERT new
app.MapPost("/employees/", async (Employee employee, OfficeDb db) =>
{
    db.Employees.Add(employee);
    await db.SaveChangesAsync();
    return Results.Created($"/employee/{employee.Id}", employee);
});

// GET one
app.MapGet("/employees/{id:int}", async (int id, OfficeDb db) =>
{
    return await db.Employees.FindAsync(id)
        is Employee employee ? Results.Ok(employee) : Results.NotFound();
});

// GET all
app.MapGet("/employees", async (OfficeDb db) => await db.Employees.ToListAsync());

// PUT
app.MapPut("/employees/{id:int}", async (int id, Employee e, OfficeDb db) =>
{
    if (e.Id != id) return Results.BadRequest();
    Employee employee = await db.Employees.FindAsync(id);
    if (employee is null) return Results.NotFound();

    employee.Name = e.Name;
    employee.Lastname = e.Lastname;
    employee.Branch = e.Branch;
    employee.Age = e.Age;

    await db.SaveChangesAsync();
    return Results.Ok(employee);
});

// DELETE
app.MapDelete("/employee/{id:int}", async (int id, OfficeDb db) =>
{
    Employee employee = await db.Employees.FindAsync(id);
    if (employee is null) return Results.NotFound();
    db.Remove(employee);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

/* ============================================================= */

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}