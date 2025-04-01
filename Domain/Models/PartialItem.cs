namespace Domain.Models;

public class PartialItem
{
    // ReSharper disable once UnusedMember.Global - Used in Application/Commands/UpdateItem.cs
    public string? Name { get; init; }
    public dynamic? Data { get; init; }
}