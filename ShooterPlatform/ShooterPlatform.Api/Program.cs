using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
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
using ShooterPlatform.Api.Infrastructure;
using ShooterPlatform.Api.MiddleWares;
using ShooterPlatform.Api.Options;
using System.Text;
using System.Threading.RateLimiting;

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

var redisConnection = builder.Configuration.GetConnectionString("Redis");

if (!string.IsNullOrWhiteSpace(redisConnection))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.ConfigurationOptions =
            new StackExchange.Redis.ConfigurationOptions
            {
                EndPoints = { redisConnection },

                ConnectTimeout = 1000,
                AsyncTimeout = 1000,
                SyncTimeout = 1000,

                AbortOnConnectFail = false
            };

        options.InstanceName = "ShooterPlatform:";
    });
}
else
{
    builder.Services.AddDistributedMemoryCache();
}

// ========================
// DI
// ========================
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IOverwatchService, OverwatchService>();
builder.Services.AddScoped<IProfileAnalysisService, ProfileAnalysisService>();
builder.Services.AddScoped<IProfileAnalysisRule, WinRateRule>();
builder.Services.AddScoped<IProfileAnalysisRule, OneTrickRule>();
builder.Services.AddScoped<IProfileAnalysisRule, AccuracyRule>();
builder.Services.AddScoped<IProfileCacheService, ProfileCacheService>();
builder.Services.AddScoped<IAnalysisResultService, AnalysisResultService>();
builder.Services.AddScoped<IAnalysisRefreshService, AnalysisRefreshService>();
builder.Services.AddScoped<IAnalysisJobService, AnalysisJobService>();

builder.Services.AddHttpClient<IOverwatchProfileProvider, OverwatchProfileProvider>();

// ========================
// Hangfire
// ========================
//builder.Services.AddHangfire(config =>
//{
//    config.UseSqlServerStorage(
//        builder.Configuration.GetConnectionString("DefaultConnection"),
//        new Hangfire.SqlServer.SqlServerStorageOptions
//        {
//            PrepareSchemaIfNecessary = false,
//            QueuePollInterval = TimeSpan.FromSeconds(30),
//            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5)
//        });
//});

//builder.Services.AddHangfireServer();

// ========================
// DB
// ========================
builder.Services.AddDbContext<ShooterPlatformDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ========================
// Rate Limiter
// ========================
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            });
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
   
    options.AddFixedWindowLimiter("profile-limit", opt =>
    {
        opt.PermitLimit = 30;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 0;
    });

    options.AddFixedWindowLimiter("analysis-limit", opt =>
    {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueLimit = 0;
    });

    options.AddFixedWindowLimiter("auth-limit", opt =>
    {
        opt.PermitLimit = 35;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueLimit = 0;
    });
});

// ========================
// FluentValidation
// ========================
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

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

// ========================
// CORS
// ========================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ========================
// HealthChecks
// ========================

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<ShooterPlatformDbContext>();

var app = builder.Build();

app.MapHealthChecks("/health");

// DB migration
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ShooterPlatformDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Migration failed");
    }
}

// Hangfire job
//using (var scope = app.Services.CreateScope())
//{
//    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

//    try
//    {
//        var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

//        recurringJobManager.AddOrUpdate<IAnalysisJobService>(
//            "refresh-analysis",
//            x => x.RefreshAllAsync(),
//            "0 3 * * *");
//    }
//    catch (Exception ex)
//    {
//        logger.LogError(ex, "Hangfire init failed");
//    }
//}


app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowReact");

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();