namespace BookStore.IdentityService.Domain.Entities;

public sealed class UserAccount
{
    public Guid Id { get; init; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;

    public static UserAccount Create(string fullName, string email, string passwordHash, string role)
    {
        return new UserAccount
        {
            Id = Guid.NewGuid(),
            FullName = fullName,
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            Role = role
        };
    }
}
