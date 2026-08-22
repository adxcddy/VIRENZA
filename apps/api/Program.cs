using QuestPDF.Infrastructure;
using Virenza.Api.Services.Learning;
using Virenza.Api.Services.Research;
using Virenza.Api.Services.Payments;
using Virenza.Api.Configuration.Payments;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Virenza.Api.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Virenza.Api.Data;
using Virenza.Api.Models.Research;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MtnPaymentOptions>(
    builder.Configuration.GetSection("Payments:MTN"));

builder.Services.Configure<AirtelPaymentOptions>(
    builder.Configuration.GetSection("Payments:Airtel"));

builder.Services.AddHttpClient<MtnPaymentProvider>();
builder.Services.AddHttpClient<AirtelMoneyPaymentProvider>();

builder.Services.AddScoped<IPaymentProvider, MtnPaymentProvider>();
builder.Services.AddScoped<IPaymentProvider, AirtelMoneyPaymentProvider>();

builder.Services.AddScoped<PaymentProviderResolver>();
builder.Services.AddScoped<PaymentService>();

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddScoped<
    ICertificatePdfService,
    CertificatePdfService>();

var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("JWT secret is not configured.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "VIRENZA";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "VIRENZA.Platform";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret)
            ),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<
    Virenza.Api.Services.Auth.IAuthService,
    Virenza.Api.Services.Auth.AuthService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<VirenzaDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IResearchService, ResearchService>();
builder.Services.AddHttpClient<IOpenAlexService, OpenAlexService>(
    client =>
    {
        client.BaseAddress = new Uri("https://api.openalex.org/");
        client.Timeout = TimeSpan.FromSeconds(30);
    });

builder.Services.AddHttpClient<ICrossrefService, CrossrefService>(
    client =>
    {
        client.BaseAddress = new Uri("https://api.crossref.org/");
        client.Timeout = TimeSpan.FromSeconds(30);
        client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "VirenzaResearchBot/1.0 (research ingestion)");
        client.DefaultRequestHeaders.Accept.ParseAdd(
            "application/json");
    });


builder.Services.AddHttpClient<IPubMedService, PubMedService>(
    client =>
    {
        client.BaseAddress = new Uri(
            "https://eutils.ncbi.nlm.nih.gov/");
        client.Timeout = TimeSpan.FromSeconds(60);
        client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "VirenzaResearchBot/1.0 (research ingestion)");
        client.DefaultRequestHeaders.Accept.ParseAdd(
            "application/json");
    });

builder.Services.AddHttpClient<IZenodoService, ZenodoService>(
    client =>
    {
        client.BaseAddress = new Uri("https://zenodo.org/");
        client.Timeout = TimeSpan.FromSeconds(60);
        client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "VirenzaResearchBot/1.0 (research ingestion)");
        client.DefaultRequestHeaders.Accept.ParseAdd(
            "application/json");
    })
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        ConnectCallback = async (context, cancellationToken) =>
        {
            var addresses = await System.Net.Dns.GetHostAddressesAsync(
                context.DnsEndPoint.Host,
                cancellationToken);

            var ipv4 = addresses.FirstOrDefault(
                x => x.AddressFamily ==
                     System.Net.Sockets.AddressFamily.InterNetwork);

            if (ipv4 is null)
            {
                throw new System.Net.Sockets.SocketException(
                    (int)System.Net.Sockets.SocketError.HostNotFound);
            }

            var socket = new System.Net.Sockets.Socket(
                System.Net.Sockets.AddressFamily.InterNetwork,
                System.Net.Sockets.SocketType.Stream,
                System.Net.Sockets.ProtocolType.Tcp);

            try
            {
                await socket.ConnectAsync(
                    new System.Net.IPEndPoint(
                        ipv4,
                        context.DnsEndPoint.Port),
                    cancellationToken);

                return new System.Net.Sockets.NetworkStream(
                    socket,
                    ownsSocket: true);
            }
            catch
            {
                socket.Dispose();
                throw;
            }
        }
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VirenzaDbContext>();

    await db.Database.MigrateAsync();

    await VirenzaEducationSeed.SeedAsync(db);
    await VirenzaCurriculumSeed.SeedAsync(db);
    await VirenzaCurriculumSubjectSeed.SeedAsync(db);
    await VirenzaLearningSeed.SeedAsync(db);
    await VirenzaResearchSeed.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapGet("/", () => Results.Ok(new
{
    service = "VIRENZA API",
    status = "online",
    version = "1.0",
    environment = app.Environment.EnvironmentName,
    health = "/health"
}));

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    service = "VIRENZA API"
}));

app.Run();
