namespace BookStore.BuildingBlocks.Contracts.Inventory;

public sealed record InventoryItemDto(Guid BookId, string Sku, int AvailableQuantity, int ReservedQuantity);

public sealed record UpsertInventoryRequest(Guid BookId, string Sku, int AvailableQuantity);

public sealed record ReserveInventoryItem(Guid BookId, int Quantity);

public sealed record ReserveInventoryRequest(Guid OrderId, IReadOnlyCollection<ReserveInventoryItem> Items);

public sealed record ReserveInventoryResponse(bool IsSuccess, string Message);
