namespace Domain.Models;

public class BaseItem
{
    public required string Name { get; set; }
    public dynamic? Data { get; set; }
}