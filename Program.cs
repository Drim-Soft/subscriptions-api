using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using SubscriptionsApi.Data;
using SubscriptionsApi.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


Env.Load();


var resendKey = Environment.GetEnvironmentVariable("RESEND_API_KEY");
var connString = Environment.GetEnvironmentVariable("CONNECTION_STRING");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connString)
);


builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.WriteIndented = true;
    });

// cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpClient<EmailService>(client =>
{
    client.BaseAddress = new Uri("https://api.resend.com");
    client.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", resendKey);
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
