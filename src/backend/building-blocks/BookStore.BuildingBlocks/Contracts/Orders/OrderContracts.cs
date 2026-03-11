namespace BookStore.BuildingBlocks.Contracts.Orders;

public sealed record OrderItemRequest(
    Guid BookId,
    string Title,
    decimal UnitPrice,
    int Quantity);

public sealed record CreateOrderRequest(IReadOnlyCollection<OrderItemRequest> Items);

public sealed record OrderItemDto(
    Guid BookId,
    string Title,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal);

public sealed record OrderDto(
    Guid Id,
    Guid UserId,
    string Status,
    decimal TotalAmount,
    DateTime CreatedAtUtc,
    IReadOnlyCollection<OrderItemDto> Items);

public sealed record UpdateOrderStatusRequest(string Status);
