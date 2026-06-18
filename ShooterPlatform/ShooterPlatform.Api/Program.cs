using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
using ShooterPlatform.Api.Application.Features.Analysis.Rules;
using ShooterPlatform.Api.Application.Features.Analysis.Services;
using ShooterPlatform.Api.Application.Features.Auth.Interfaces;
using ShooterPlatform.Api.Application.Features.Auth.Services;
using ShooterPlatform.Api.Application.Features.Favorite.Interfaces;
using ShooterPlatform.Api.Application.Features.Favorite.Services;
using ShooterPlatform.Api.Application.Features.Overwatch.Interfaces;
using ShooterPlatform.Api.Application.Features.Overwatch.Providers;
using ShooterPlatform.Api.Application.Features.Overwatch.Services;
using ShooterPlatform.Api.Application.Features.Players.Interfaces;
using ShooterPlatform.Api.Application.Features.Players.Services;
using ShooterPlatform.Api.Application.Features.Players.Validators;
using ShooterPlatform.Api.Infrastructure;
using ShooterPlatform.Api.MiddleWares;
using ShooterPlatform.Api.Options;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ========================
// Controllers
// ========================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        );
    });

// ========================
// Swagger + JWT Support
// ========================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ========================
// JWT Settings
// ========================
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

// ========================
// Cache
// ========================
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "ShooterPlatform:";
});

// ========================
// DI
// ========================
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IOverwatchService, OverwatchService>();
builder.Services.AddScoped<IProfileAnalysisService, ProfileAnalysisService>();
builder.Services.AddScoped<IProfileAnalysisRule, WinRateRule>();
builder.Services.AddScoped<IProfileAnalysisRule, OneTrickRule>();
builder.Services.AddScoped<IProfileAnalysisRule, AccuracyRule>();
builder.Services.AddScoped<IProfileCacheService, ProfileCacheService>();
builder.Services.AddScoped<IBatchProfileAnalysisService, BatchProfileAnalysisService>();

builder.Services.AddHttpClient<IOverwatchProfileProvider, OverwatchProfileProvider>();

// ========================
// DB
// ========================
builder.Services.AddDbContext<ShooterPlatformDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ========================
// FluentValidation
// ========================
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<PlayerQueryRequestValidator>();

// ========================
// JWT Authentication
// ========================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var settings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = settings.Issuer,
        ValidAudience = settings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(settings.SecretKey)),

        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("JWT FAILED: " + context.Exception.Message);
            return Task.CompletedTask;
        },

        OnChallenge = context =>
        {
            Console.WriteLine("JWT CHALLENGE TRIGGERED");
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();