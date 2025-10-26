using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using SubscriptionsApi.Data;


var builder = WebApplication.CreateBuilder(args);

// Cargar variables del archivo .env
Env.Load();

// Leer la cadena desde el .env
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
