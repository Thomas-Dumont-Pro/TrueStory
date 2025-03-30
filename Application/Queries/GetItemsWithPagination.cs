﻿using Application.Common.Enums;
using Application.Common.Models;
using Domain.Models;
using MediatR;

namespace Application.Queries;

public sealed class GetItemsWithPagination : IRequest<IEnumerable<Item>>
{
    public List<string>? ListId { get; init; }
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required SortOrder SortOrder { get; init; }
}

// ReSharper disable once UnusedMember.Global - Used in Api/Controller/ItemController.cs by mediator.Send
public sealed class GetItemsWithPaginationHandler(ItemRepository itemRepository)
    : IRequestHandler<GetItemsWithPagination, IEnumerable<Item>>
{
    public async Task<IEnumerable<Item>> Handle(GetItemsWithPagination request, CancellationToken cancellationToken)
    {
        var data = await itemRepository.GetItems(cancellationToken);

        var result = data
           .Where(x => request.ListId?.Count == 0 || (request.ListId?.Contains(x.Id) ?? true));

        result = request.SortOrder == SortOrder.Ascending ? result.OrderBy(x => x.Name) : result.OrderByDescending(x => x.Name);

        return result.Skip(request.PageNumber * request.PageSize)
        .Take(request.PageSize);
    }
}