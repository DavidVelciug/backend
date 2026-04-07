using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyFullstackApp.Domains.Entities.Product;

namespace MyFullstackApp.Domains.Entities.Category;

public class CategoryData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public List<ProductData> Products { get; set; } = new();
}
