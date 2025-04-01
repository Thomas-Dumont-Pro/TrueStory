namespace Domain.Models;

public class BaseItem : PartialItem
{
    public new required string Name { get; set; }

    // ReSharper disable once UnusedMember.Global - Used in Application/Commands/CreateItem.cs
    public DateTimeOffset? CreatedAt { get; set; }

    // ReSharper disable once UnusedMember.Global - Used in Application/Commands/UpdateItem.cs
    public DateTimeOffset? UpdatedAt { get; set; }
}