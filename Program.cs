using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using SubscriptionsApi.Data;
using Prometheus;


var builder = WebApplication.CreateBuilder(args);

// Cargar variables del archivo .env
Env.Load();

// Leer la cadena desde el .env
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar métricas de Prometheus
app.UseMetricServer(); // Expone el endpoint /metrics
app.UseHttpMetrics(); // Métricas HTTP automáticas

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
