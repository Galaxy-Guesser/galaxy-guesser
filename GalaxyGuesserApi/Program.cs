using GalaxyGuesserApi.Data;
using GalaxyGuesserApi.Repositories;
using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DatabaseContext>();

builder.Services.AddScoped<IPlayerRepository,PlayerRepository>();
builder.Services.AddScoped<ISessionRepository,SessionRepository>();
builder.Services.AddScoped<ISessionViewRepository, SessionViewRepository>();

builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<SessionViewService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();