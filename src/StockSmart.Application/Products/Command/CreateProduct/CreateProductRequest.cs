using System.ComponentModel.DataAnnotations;

namespace StockSmart.Application.Products.Command.CreateProduct;

public class CreateProductRequest
{
    [Required]
    [MaxLength(30)]
    public string ProductCode { get; set; }

    [Required]
    [MaxLength(500)]
    public string Name { get; set; }

    [Required]
    public string StatusName { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Required]
    [MaxLength(5000)]
    public string Description { get; set; }

    [Required]
    [Range(0, (double)decimal.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, (double)decimal.MaxValue)]
    public decimal? Weight { get; set; }

    [Range(0, (double)decimal.MaxValue)]
    public decimal? Size { get; set; }
}
