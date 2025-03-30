namespace Domain.Models;

public class BaseItem
{
    public required string Name { get; set; }
    public object? Data { get; set; }
}