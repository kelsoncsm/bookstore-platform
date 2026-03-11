using BookStore.CatalogService.Application.Books;
using BookStore.CatalogService.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.CatalogService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICatalogService, InMemoryCatalogService>();
        return services;
    }
}
