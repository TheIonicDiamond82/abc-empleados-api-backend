using EmployeesManagement.API.Data;
using EmployeesManagement.API.Services;          
using EmployeesManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar conexión a PostgreSQL (Supabase) con Npgsql
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. Controladores y OpenAPI nativo (.NET 10)
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 3. Configurar CORS para Vue 3 (Vite en puerto 5173)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueClient", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 4. Registrar servicios de la aplicación
builder.Services.AddScoped<IDepartmentsService, DepartmentsService>();
builder.Services.AddScoped<IPositionsService, PositionsService>();
builder.Services.AddScoped<IEmployeesService, EmployeesService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Fuerza el formato camelCase en todas las respuestas JSON de la API
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        
        // Opcional: maneja enums como strings si usas estatus en texto
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

// 4. Middleware de desarrollo, OpenAPI y Scalar
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Employees Management API")
            .WithTheme(ScalarTheme.Mars);
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowVueClient");

app.UseAuthorization();

// 5. Endpoint de prueba de conexión a Supabase
app.MapGet("/api/health/db-check", async (ApplicationDbContext context) =>
{
    try
    {
        var canConnect = await context.Database.CanConnectAsync();
        if (canConnect)
        {
            var employeesCount = await context.Employees.CountAsync();
            var departmentsNames = await context.Departments.Select(d => d.Name).ToListAsync();
            return Results.Ok(new 
            { 
                status = "Healthy", 
                message = "Conexión exitosa a Supabase PostgreSQL con .NET 10 y Scalar", 
                employeesCount, 
                departmentsNames
            });

        }
        return Results.Problem("No se pudo conectar a la base de datos.");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error de conexión: {ex.Message}");
    }
});

app.MapControllers();

app.Run();