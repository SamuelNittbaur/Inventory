using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Globalization;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Google.Protobuf.WellKnownTypes;
using Microsoft.SqlServer.Server;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddConsoleExporter(); // Export metrics to the console
    })
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter(); // Export traces to the console
    })
    .ConfigureResource(resource =>
        resource.AddService(serviceName: "api"))
    .UseOtlpExporter();

var app = builder.Build();

var environment = builder.Environment;
string relativePath = Environment.GetEnvironmentVariable("relativePath");
string fullPath = Path.Combine(environment.WebRootPath, relativePath);
if (app.Environment.IsDevelopment())
{
    relativePath = Environment.GetEnvironmentVariable("fbFile");
}
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", relativePath, EnvironmentVariableTarget.Process);

app.UseMiddleware<SecurityHeaderMiddleware>();
app.UseMiddleware<IpRateLimitingMiddleware>(50, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(3));
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        // Log exception here
        await context.Response.WriteAsync("An error occurred.");
    });
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllers();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

await DataBase.UserDataBase.CreateFireBaseSecureLogin();

app.Run();



public class SecurityHeaderMiddleware
{
    private readonly RequestDelegate _next;
    private string SECURITY_HEADER_KEY = "X-Security-Key";
    private string SECURITY_HEADER_CONTENT = "X-Security-Value";

    public SecurityHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {

        if (context.Request.Method != "OPTIONS" && !context.Request.Path.Value.Contains("swagger") && !context.Request.Path.Value.Contains("health") && !context.Request.Path.Value.Contains("sendTestMail"))
        {


            if (!context.Request.Headers.TryGetValue(SECURITY_HEADER_KEY, out var extractedToken))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Security header missing.");
                return;
            }

            if (!context.Request.Headers.TryGetValue(SECURITY_HEADER_CONTENT, out var extractedTokenContent))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Security header missing.");
                return;
            }

            extractedToken = BusinessLogic.Cryption.CryptionLogic.Decrypt(extractedToken, BusinessLogic.Cryption.CryptionMethod.secOne);
            string guid = extractedToken.ToString().Split("?").First();
            string date = extractedToken.ToString().Split("?").Last();
            extractedTokenContent = BusinessLogic.Cryption.CryptionLogic.Decrypt(extractedTokenContent, BusinessLogic.Cryption.CryptionMethod.secTwo);

            var guidValidation = !Guid.TryParse(guid, out _);
            var parsedDate = DateTime.ParseExact(date, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            var dateValidation = (DateTime.Now - parsedDate).TotalSeconds >= 15;
            var contentDateValid = (DateTime.ParseExact(extractedTokenContent, "dd.MM.yyyy", CultureInfo.InvariantCulture) - DateTime.Now).TotalSeconds >= 45;

            if (guidValidation || dateValidation || contentDateValid)
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Invalid security token.");
                return;
            }

            await _next(context); // Call the next middleware in the pipeline
        }
        else
        {
            await _next(context);
        }
    }
}

public class IpRateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<string, (DateTime LastRequest, int RequestCount)> _ipRequests
        = new ConcurrentDictionary<string, (DateTime, int)>();
    private static readonly ConcurrentDictionary<string, DateTime> _blockedIps
        = new ConcurrentDictionary<string, DateTime>();

    private readonly int _requestLimit;
    private readonly TimeSpan _timeWindow;
    private readonly TimeSpan _blockDuration;

    public IpRateLimitingMiddleware(RequestDelegate next, int requestLimit, TimeSpan timeWindow, TimeSpan blockDuration)
    {
        _next = next;
        _requestLimit = requestLimit;
        _timeWindow = timeWindow;
        _blockDuration = blockDuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(ip))
        {
            await _next(context);
            return;
        }

        // Check if IP is blocked
        if (_blockedIps.TryGetValue(ip, out var blockedUntil))
        {
            if (DateTime.UtcNow < blockedUntil)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Too many requests. Try again later.");
                return;
            }
            else
            {
                _blockedIps.TryRemove(ip, out _);
            }
        }

        // Rate limiting logic
        var now = DateTime.UtcNow;

        var entry = _ipRequests.GetOrAdd(ip, _ => (now, 0));

        if (now - entry.LastRequest > _timeWindow)
        {
            _ipRequests[ip] = (now, 1);
        }
        else
        {
            _ipRequests[ip] = (entry.LastRequest, entry.RequestCount + 1);

            if (entry.RequestCount + 1 > _requestLimit)
            {
                _blockedIps[ip] = now.Add(_blockDuration);
                _ipRequests.TryRemove(ip, out _);

                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Too many requests. Try again later.");
                return;
            }
        }

        await _next(context);
    }
}