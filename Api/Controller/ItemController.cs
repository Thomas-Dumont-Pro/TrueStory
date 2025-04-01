using Application.Commands;
using Application.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Queries;
using Domain.Models;

namespace Api;

[ApiController]
[Route("[controller]")]
public class ItemController(IMediator mediator) : ControllerBase
{


    // GET: ItemController
    [HttpGet(Name = "Get All Objects")]
    public async Task<ActionResult> GetAll([FromQuery] int pageSize=10, [FromQuery] int pageNumber=0, [FromQuery] string[]? ids = null, [FromQuery] SortOrder sortOrder = SortOrder.Ascending)
    {
        return Ok(await mediator.Send(new GetItemsWithPagination { PageNumber = pageNumber, PageSize = pageSize, ListId = ids?.ToList(), SortOrder = sortOrder}));
    }

    // GET: ItemController/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Details(string id)
    {
        return Ok(await mediator.Send(new GetItem(id)));
    }

    // POST: ItemController
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> Create([FromBody] BaseItem item)
    {
        var createdItem = await mediator.Send(new CreateItem(item));
        return CreatedAtAction(nameof(Details), new { id = createdItem.Id }, createdItem);
    }

    // PATCH: ItemController/5
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    public async Task<ActionResult> Edit(string id, [FromBody] PartialItem item)
    {
        var updatedItem = await mediator.Send(new UpdateItem(id,item));
        return AcceptedAtAction(nameof(Details), new { id = updatedItem.Id }, updatedItem);
    }

    // DELETE : ItemController/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(string id)
    {
        await mediator.Send(new DeleteItem(id));
        return NoContent();
    }
}
