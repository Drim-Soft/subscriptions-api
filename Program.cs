using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using SubscriptionsApi.Data;
using Prometheus;
using Npgsql;


var builder = WebApplication.CreateBuilder(args);

// Cargar variables del archivo .env
Env.Load();

// Leer la cadena desde el .env
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

// Construir el NpgsqlConnectionStringBuilder para configurar parámetros de conexión
var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString)
{
    // Timeout de conexión inicial (30 segundos)
    Timeout = 30,
    // Timeout de comando (120 segundos)
    CommandTimeout = 120,
    // Pool de conexiones optimizado
    MinPoolSize = 1,
    MaxPoolSize = 20,
    // Buffers más grandes para mejor rendimiento en conexiones lentas
    ReadBufferSize = 16384,  // 16KB
    WriteBufferSize = 16384   // 16KB
};

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionStringBuilder.ConnectionString, npgsqlOptions =>
    {
        // Timeout de comandos a 120 segundos (2 minutos)
        npgsqlOptions.CommandTimeout(120);
        
        // Habilitar reintentos automáticos para errores transitorios
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null);
    });
    
    // Solo para desarrollo - remover en producción
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS para permitir cualquier origen
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configurar métricas de Prometheus
app.UseMetricServer(); // Expone el endpoint /metrics
app.UseHttpMetrics(); // Métricas HTTP automáticas

app.UseSwagger();
app.UseSwaggerUI();

// Habilitar CORS
app.UseCors();

app.MapControllers();

app.Run();
