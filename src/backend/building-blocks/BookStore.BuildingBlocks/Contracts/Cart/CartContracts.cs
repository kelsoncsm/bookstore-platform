namespace BookStore.BuildingBlocks.Contracts.Cart;

public sealed record CartItemDto(
    Guid BookId,
    string Title,
    string Author,
    decimal UnitPrice,
    int Quantity,
    string CoverImageUrl);

public sealed record CartDto(
    Guid UserId,
    IReadOnlyCollection<CartItemDto> Items,
    decimal TotalAmount);

public sealed record AddCartItemRequest(
    Guid BookId,
    string Title,
    string Author,
    decimal UnitPrice,
    int Quantity,
    string CoverImageUrl);

public sealed record UpdateCartItemQuantityRequest(int Quantity);
