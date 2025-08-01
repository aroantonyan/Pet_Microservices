namespace ProductService.Contracts.Common;

public class RequestResponseDto<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
}