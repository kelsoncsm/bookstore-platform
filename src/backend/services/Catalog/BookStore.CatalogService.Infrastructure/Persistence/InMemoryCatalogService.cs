using System.Collections.Concurrent;
using BookStore.BuildingBlocks.Contracts.Catalog;
using BookStore.CatalogService.Application.Books;
using BookStore.CatalogService.Domain.Entities;

namespace BookStore.CatalogService.Infrastructure.Persistence;

public sealed class InMemoryCatalogService : ICatalogService
{
    private static readonly ConcurrentDictionary<Guid, Author> Authors = new();
    private static readonly ConcurrentDictionary<Guid, Category> Categories = new();
    private static readonly ConcurrentDictionary<Guid, Book> Books = new();

    static InMemoryCatalogService()
    {
        var cleanCodeAuthor = new Author(Guid.NewGuid(), "Robert C. Martin");
        var pragmaticProgrammerAuthor = new Author(Guid.NewGuid(), "Andrew Hunt");
        var softwareCategory = new Category(Guid.NewGuid(), "Software Engineering");
        var architectureCategory = new Category(Guid.NewGuid(), "Architecture");

        Authors.TryAdd(cleanCodeAuthor.Id, cleanCodeAuthor);
        Authors.TryAdd(pragmaticProgrammerAuthor.Id, pragmaticProgrammerAuthor);
        Categories.TryAdd(softwareCategory.Id, softwareCategory);
        Categories.TryAdd(architectureCategory.Id, architectureCategory);

        var cleanCode = Book.Create(
            "Clean Code",
            "A handbook of agile software craftsmanship.",
            cleanCodeAuthor.Id,
            softwareCategory.Id,
            "9780132350884",
            149.90m,
            25,
            "https://images.unsplash.com/photo-1512820790803-83ca734da794");

        var pragmaticProgrammer = Book.Create(
            "The Pragmatic Programmer",
            "Classic practices for modern software teams.",
            pragmaticProgrammerAuthor.Id,
            architectureCategory.Id,
            "9780135957059",
            169.90m,
            18,
            "https://images.unsplash.com/photo-1495446815901-a7297e633e8d");

        Books.TryAdd(cleanCode.Id, cleanCode);
        Books.TryAdd(pragmaticProgrammer.Id, pragmaticProgrammer);
    }

    public Task<BookDetailsDto> CreateBookAsync(UpsertBookRequest request, CancellationToken cancellationToken = default)
    {
        EnsureReferencesExist(request.AuthorId, request.CategoryId);
        var book = Book.Create(
            request.Title,
            request.Description,
            request.AuthorId,
            request.CategoryId,
            request.Isbn,
            request.Price,
            request.AvailableQuantity,
            request.CoverImageUrl);

        Books[book.Id] = book;
        return Task.FromResult(ToDetailsDto(book));
    }

    public Task<bool> DeleteBookAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Books.TryRemove(bookId, out _));
    }

    public Task<IReadOnlyCollection<AuthorDto>> GetAuthorsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<AuthorDto>>(Authors.Values
            .OrderBy(item => item.Name)
            .Select(item => new AuthorDto(item.Id, item.Name))
            .ToArray());
    }

    public Task<BookDetailsDto?> GetBookAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Books.TryGetValue(bookId, out var book) ? ToDetailsDto(book) : null);
    }

    public Task<IReadOnlyCollection<BookSummaryDto>> GetBooksAsync(string? title, string? author, string? category, string? isbn, CancellationToken cancellationToken = default)
    {
        var query = Books.Values.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(item => item.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            query = query.Where(item => Authors[item.AuthorId].Name.Contains(author, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(item => Categories[item.CategoryId].Name.Contains(category, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(isbn))
        {
            query = query.Where(item => item.Isbn.Contains(isbn, StringComparison.OrdinalIgnoreCase));
        }

        var items = query
            .OrderBy(item => item.Title)
            .Select(item => new BookSummaryDto(
                item.Id,
                item.Title,
                Authors[item.AuthorId].Name,
                Categories[item.CategoryId].Name,
                item.Isbn,
                item.Price,
                item.AvailableQuantity,
                item.CoverImageUrl))
            .ToArray();

        return Task.FromResult<IReadOnlyCollection<BookSummaryDto>>(items);
    }

    public Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<CategoryDto>>(Categories.Values
            .OrderBy(item => item.Name)
            .Select(item => new CategoryDto(item.Id, item.Name))
            .ToArray());
    }

    public Task<BookDetailsDto?> UpdateBookAsync(Guid bookId, UpsertBookRequest request, CancellationToken cancellationToken = default)
    {
        EnsureReferencesExist(request.AuthorId, request.CategoryId);
        if (!Books.TryGetValue(bookId, out var book))
        {
            return Task.FromResult<BookDetailsDto?>(null);
        }

        book.Update(
            request.Title,
            request.Description,
            request.AuthorId,
            request.CategoryId,
            request.Isbn,
            request.Price,
            request.AvailableQuantity,
            request.CoverImageUrl);

        return Task.FromResult<BookDetailsDto?>(ToDetailsDto(book));
    }

    private static void EnsureReferencesExist(Guid authorId, Guid categoryId)
    {
        if (!Authors.ContainsKey(authorId) || !Categories.ContainsKey(categoryId))
        {
            throw new InvalidOperationException("Author or category was not found.");
        }
    }

    private static BookDetailsDto ToDetailsDto(Book book)
    {
        return new BookDetailsDto(
            book.Id,
            book.Title,
            book.Description,
            Authors[book.AuthorId].Name,
            Categories[book.CategoryId].Name,
            book.Isbn,
            book.Price,
            book.AvailableQuantity,
            book.CoverImageUrl);
    }
}
