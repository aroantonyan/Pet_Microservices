namespace ProductService.Contracts.Products.Dtos;

public class ProductResponseDto
{
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public double? ProductPrice { get; set; }
}