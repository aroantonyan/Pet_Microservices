namespace ProductService.Contracts.Authentication;

public class RefreshAccessTokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}