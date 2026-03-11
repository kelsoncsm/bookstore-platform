namespace BookStore.BuildingBlocks.Contracts.Auth;

public sealed record RegisterRequest(string FullName, string Email, string Password);

public sealed record LoginRequest(string Email, string Password);

public sealed record AuthResponse(Guid UserId, string FullName, string Email, string Role, string AccessToken, DateTime ExpiresAtUtc);

public sealed record UserProfileResponse(Guid UserId, string FullName, string Email, string Role);
