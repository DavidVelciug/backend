namespace MyFullstackApp.Domains.Models.Product;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public int CategoryId { get; set; }

    /// <summary>Имя категории (подставляется при чтении из БД).</summary>
    public string? Category { get; set; }
}
