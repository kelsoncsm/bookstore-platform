namespace BookStore.CatalogService.Domain.Entities;

public sealed class Book
{
    public Guid Id { get; init; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Guid AuthorId { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Isbn { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int AvailableQuantity { get; private set; }
    public string CoverImageUrl { get; private set; } = string.Empty;

    public static Book Create(
        string title,
        string description,
        Guid authorId,
        Guid categoryId,
        string isbn,
        decimal price,
        int availableQuantity,
        string coverImageUrl)
    {
        return new Book
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            AuthorId = authorId,
            CategoryId = categoryId,
            Isbn = isbn,
            Price = price,
            AvailableQuantity = availableQuantity,
            CoverImageUrl = coverImageUrl
        };
    }

    public void Update(
        string title,
        string description,
        Guid authorId,
        Guid categoryId,
        string isbn,
        decimal price,
        int availableQuantity,
        string coverImageUrl)
    {
        Title = title;
        Description = description;
        AuthorId = authorId;
        CategoryId = categoryId;
        Isbn = isbn;
        Price = price;
        AvailableQuantity = availableQuantity;
        CoverImageUrl = coverImageUrl;
    }
}
