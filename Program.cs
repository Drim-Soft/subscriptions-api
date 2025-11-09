using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using SubscriptionsApi.Data;


var builder = WebApplication.CreateBuilder(args);

// Cargar variables del archivo .env
Env.Load();

// Leer la cadena desde el .env
var connectionString = Environment.GetEnvironmentVariable("SERVER_SERVLET_CONTEXT_PATH");

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
builder.Services.AddScoped<EmailService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
