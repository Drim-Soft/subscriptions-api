using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using SubscriptionsApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables del .env
Env.Load();

// Obtener variables
var dbUrl = Environment.GetEnvironmentVariable("DB_URL");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

// Convertir la URL tipo JDBC en formato Npgsql
// (solo si la variable empieza por jdbc:)
if (dbUrl != null && dbUrl.StartsWith("jdbc:"))
{
    dbUrl = dbUrl.Replace("jdbc:", ""); // quita el prefijo JDBC
}

// Construir manualmente la cadena si prefieres control total
// o usar directamente la que tienes en DB_URL
var connectionString = $"{dbUrl};Username={dbUser};Password={dbPassword}";

// Inicializar EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Resto de la configuración
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// (Opcional) Aplicar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
