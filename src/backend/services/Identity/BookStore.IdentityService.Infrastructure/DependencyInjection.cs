using BookStore.IdentityService.Application.Authentication;
using BookStore.IdentityService.Infrastructure.Persistence;
using BookStore.IdentityService.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.IdentityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new JwtTokenFactory(configuration));
        services.AddSingleton<IIdentityService, InMemoryIdentityService>();
        return services;
    }
}
