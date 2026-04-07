using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyFullstackApp.Domains.Entities.Category;

namespace MyFullstackApp.Domains.Entities.Product;

public class ProductData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    [StringLength(4000)]
    public string? Description { get; set; }

    [StringLength(1000)]
    public string? Image { get; set; }

    public int CategoryId { get; set; }
    public CategoryData Category { get; set; } = null!;
}
