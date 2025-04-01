namespace Domain.Models;

public class BaseItem : PartialItem
{
    public new required string Name { get; set; }
    
    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
}