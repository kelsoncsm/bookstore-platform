namespace BookStore.BuildingBlocks.Contracts.Catalog;

public sealed record AuthorDto(Guid Id, string Name);

public sealed record CategoryDto(Guid Id, string Name);

public sealed record BookSummaryDto(
    Guid Id,
    string Title,
    string Author,
    string Category,
    string Isbn,
    decimal Price,
    int AvailableQuantity,
    string CoverImageUrl);

public sealed record BookDetailsDto(
    Guid Id,
    string Title,
    string Description,
    string Author,
    string Category,
    string Isbn,
    decimal Price,
    int AvailableQuantity,
    string CoverImageUrl);

public sealed record UpsertBookRequest(
    string Title,
    string Description,
    Guid AuthorId,
    Guid CategoryId,
    string Isbn,
    decimal Price,
    int AvailableQuantity,
    string CoverImageUrl);

public sealed record UpsertAuthorRequest(string Name);

public sealed record UpsertCategoryRequest(string Name);
