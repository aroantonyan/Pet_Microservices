namespace ProductService.Contracts.Products.Dtos;

public class CreateProductDto
{
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public double ProductPrice { get; set; }
    public string? Currency { get; set; }
}