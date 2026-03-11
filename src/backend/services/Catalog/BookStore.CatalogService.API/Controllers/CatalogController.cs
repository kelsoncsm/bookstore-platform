using BookStore.BuildingBlocks.Contracts.Catalog;
using BookStore.CatalogService.Application.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.CatalogService.API.Controllers;

[ApiController]
[Route("api/catalog")]
public sealed class CatalogController(ICatalogService catalogService) : ControllerBase
{
    [HttpGet("books")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyCollection<BookSummaryDto>>> GetBooks(
        [FromQuery] string? title,
        [FromQuery] string? author,
        [FromQuery] string? category,
        [FromQuery] string? isbn,
        CancellationToken cancellationToken)
    {
        return Ok(await catalogService.GetBooksAsync(title, author, category, isbn, cancellationToken));
    }

    [HttpGet("books/{bookId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<BookDetailsDto>> GetBook(Guid bookId, CancellationToken cancellationToken)
    {
        var book = await catalogService.GetBookAsync(bookId, cancellationToken);
        return book is null ? NotFound() : Ok(book);
    }

    [HttpGet("authors")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyCollection<AuthorDto>>> GetAuthors(CancellationToken cancellationToken)
    {
        return Ok(await catalogService.GetAuthorsAsync(cancellationToken));
    }

    [HttpGet("categories")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyCollection<CategoryDto>>> GetCategories(CancellationToken cancellationToken)
    {
        return Ok(await catalogService.GetCategoriesAsync(cancellationToken));
    }

    [HttpPost("books")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BookDetailsDto>> CreateBook(UpsertBookRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var book = await catalogService.CreateBookAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetBook), new { bookId = book.Id }, book);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    [HttpPut("books/{bookId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BookDetailsDto>> UpdateBook(Guid bookId, UpsertBookRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var book = await catalogService.UpdateBookAsync(bookId, request, cancellationToken);
            return book is null ? NotFound() : Ok(book);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    [HttpDelete("books/{bookId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBook(Guid bookId, CancellationToken cancellationToken)
    {
        return await catalogService.DeleteBookAsync(bookId, cancellationToken) ? NoContent() : NotFound();
    }
}
