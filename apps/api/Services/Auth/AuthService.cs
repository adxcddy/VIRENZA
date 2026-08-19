using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Virenza.Api.Data;
using Virenza.Api.DTOs.Auth;
using Virenza.Api.Models.Commerce;
using Virenza.Api.Models.Identity;

namespace Virenza.Api.Services.Auth;

public sealed class AuthService : IAuthService
{
    private readonly VirenzaDbContext _db;
    private readonly IConfiguration _configuration;

    public AuthService(
        VirenzaDbContext db,
        IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var firstName = request.FirstName.Trim();
        var lastName = request.LastName.Trim();
        var email = request.Email.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("All required registration fields must be provided.");
        }

        if (request.Password.Length < 8)
            throw new ArgumentException("Password must contain at least 8 characters.");

        var exists = await _db.Users
            .AnyAsync(x => x.Email.ToLower() == email);

        if (exists)
            throw new InvalidOperationException("An account with this email already exists.");

        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.Student,
            IsActive = true,
            EmailVerified = false,
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);

        var trial = new Trial
        {
            UserId = user.Id,
            DurationDays = 7,
            StartedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsUsed = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _db.Trials.Add(trial);

        await _db.SaveChangesAsync();

        return CreateResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email);

        if (user is null ||
            !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        if (!user.IsActive)
            throw new UnauthorizedAccessException("This account is inactive.");

        user.LastLoginAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return CreateResponse(user);
    }

    private AuthResponse CreateResponse(User user)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(
            _configuration.GetValue<int?>("Jwt:ExpiryMinutes") ?? 60
        );

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var secret = _configuration["Jwt:Secret"]
            ?? throw new InvalidOperationException("JWT secret is not configured.");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secret)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "VIRENZA",
            audience: _configuration["Jwt:Audience"] ?? "VIRENZA.Platform",
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role.ToString(),
            ExpiresAt = expiresAt
        };
    }
}
