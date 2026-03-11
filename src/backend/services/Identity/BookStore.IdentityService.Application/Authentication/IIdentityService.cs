using BookStore.BuildingBlocks.Contracts.Auth;

namespace BookStore.IdentityService.Application.Authentication;

public interface IIdentityService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<UserProfileResponse?> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default);
}
