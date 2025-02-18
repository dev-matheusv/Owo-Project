namespace Owo.Core.Models;

public class Category
{
    public long Id { get; set; }
    public String Title { get; set; } = string.Empty;
    public String? Description { get; set; }
    public String UserId { get; set; } = string.Empty;
}