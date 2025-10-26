using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using SubscriptionsApi.Data;

var builder = WebApplication.CreateBuilder(args);


Env.Load();


var dbUrl = Environment.GetEnvironmentVariable("DB_URL");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");



if (dbUrl != null && dbUrl.StartsWith("jdbc:"))
{
    dbUrl = dbUrl.Replace("jdbc:", ""); 
}



var connectionString = $"{dbUrl};Username={dbUser};Password={dbPassword}";


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


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
