using Virenza.Api.Services.Payments;
using Virenza.Api.Configuration.Payments;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Virenza.Api.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Virenza.Api.Data;

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
builder.Services.AddScoped<Virenza.Api.Services.Auth.IAuthService, Virenza.Api.Services.Auth.AuthService>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<VirenzaDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VirenzaDbContext>();

    await db.Database.EnsureCreatedAsync();

    await VirenzaEducationSeed.SeedAsync(db);
    await VirenzaCurriculumSeed.SeedAsync(db);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
