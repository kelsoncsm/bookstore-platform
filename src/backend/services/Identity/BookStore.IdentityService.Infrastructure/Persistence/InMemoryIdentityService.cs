using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using BookStore.BuildingBlocks.Contracts.Auth;
using BookStore.IdentityService.Application.Authentication;
using BookStore.IdentityService.Domain.Entities;
using BookStore.IdentityService.Infrastructure.Security;

namespace BookStore.IdentityService.Infrastructure.Persistence;

public sealed class InMemoryIdentityService(JwtTokenFactory jwtTokenFactory) : IIdentityService
{
    private static readonly ConcurrentDictionary<string, UserAccount> Users = new(StringComparer.OrdinalIgnoreCase);

    static InMemoryIdentityService()
    {
        var admin = UserAccount.Create(
            "Admin BookStore",
            "admin@bookstore.local",
            HashPassword("Admin@123"),
            "Admin");

        Users.TryAdd(admin.Email, admin);
    }

    public Task<UserProfileResponse?> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = Users.Values.FirstOrDefault(item => item.Id == userId);
        return Task.FromResult(user is null
            ? null
            : new UserProfileResponse(user.Id, user.FullName, user.Email, user.Role));
    }

    public Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (!Users.TryGetValue(request.Email.Trim().ToLowerInvariant(), out var user) ||
            user.PasswordHash != HashPassword(request.Password))
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        return Task.FromResult(CreateResponse(user));
    }

    public Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        if (Users.ContainsKey(normalizedEmail))
        {
            throw new InvalidOperationException("An account with this email already exists.");
        }

        var user = UserAccount.Create(request.FullName, normalizedEmail, HashPassword(request.Password), "Customer");
        if (!Users.TryAdd(user.Email, user))
        {
            throw new InvalidOperationException("Could not create the account.");
        }

        return Task.FromResult(CreateResponse(user));
    }

    private AuthResponse CreateResponse(UserAccount user)
    {
        var (token, expiresAtUtc) = jwtTokenFactory.CreateToken(user.Id, user.FullName, user.Email, user.Role);
        return new AuthResponse(user.Id, user.FullName, user.Email, user.Role, token, expiresAtUtc);
    }

    private static string HashPassword(string password)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(hash);
    }
}
