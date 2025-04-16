using GalaxyGuesserApi.Data;
using GalaxyGuesserApi.Repositories;
using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GalaxyGuesserApi.Configuration;


var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowCli", policy =>
//     {
//         policy.WithOrigins(
//             "http://localhost:5000"           
//         )
//         .AllowAnyMethod()         
//         .AllowAnyHeader()       
//         .AllowCredentials();
//     });
// });

var googleAuthSettings = new GoogleAuthSettings();
builder.Configuration.GetSection("Google").Bind(googleAuthSettings);

builder.Services.AddOpenApi();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddHttpClient();

builder.Services.AddSingleton(googleAuthSettings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = googleAuthSettings.authority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = googleAuthSettings.authority,
            ValidateAudience = true,
            ValidAudience = googleAuthSettings.clientId,
            ValidateLifetime = true
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("❌ Auth failed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("✅ Token validated");
                return Task.CompletedTask;
            }
        };
    })
    .AddGoogle(options =>
    { 
        options.ClientId = googleAuthSettings.clientId;
        options.ClientSecret = googleAuthSettings.clientSecret;
    });
// Add authorization
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DatabaseContext>();

builder.Services.AddScoped<IPlayerRepository,PlayerRepository>();
builder.Services.AddScoped<ISessionRepository,SessionRepository>();
builder.Services.AddScoped<ISessionViewRepository, SessionViewRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<ISessionScoreRepository, SessionScoreRepository>();

builder.Services.AddScoped<SessionScoreService>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<SessionViewService>();
builder.Services.AddScoped<QuestionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.UseCors("AllowCli");
// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();