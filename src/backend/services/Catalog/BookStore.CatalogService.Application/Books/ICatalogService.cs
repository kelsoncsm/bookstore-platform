using BookStore.BuildingBlocks.Contracts.Catalog;

namespace BookStore.CatalogService.Application.Books;

public interface ICatalogService
{
    Task<IReadOnlyCollection<BookSummaryDto>> GetBooksAsync(string? title, string? author, string? category, string? isbn, CancellationToken cancellationToken = default);
    Task<BookDetailsDto?> GetBookAsync(Guid bookId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AuthorDto>> GetAuthorsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<BookDetailsDto> CreateBookAsync(UpsertBookRequest request, CancellationToken cancellationToken = default);
    Task<BookDetailsDto?> UpdateBookAsync(Guid bookId, UpsertBookRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteBookAsync(Guid bookId, CancellationToken cancellationToken = default);
}
