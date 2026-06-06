
namespace Data.Dtos;

public class JwtTokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }     
    public String FirstName { get; set; } = String.Empty;
    public String LastName { get; set; } = String.Empty;
    public UserRole Role { get; set; }

}
