 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data.Dtos;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Infrastructure;

public class JwtTokenServices
{

    private readonly GeneralSettings generalSettings;

    public JwtTokenServices(IOptions<GeneralSettings> generalSettings)
    {
        this.generalSettings = generalSettings.Value;
    }

    public JwtTokenDto CreateToken(List<Claim> customClaims, int expiryInMinutes = 20)
    {
        var secretKey = generalSettings.JwtSettings.Key;
        var issuer = generalSettings.JwtSettings.Issuer;
        var audience = generalSettings.JwtSettings.Audience;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expirationDate = DateTime.UtcNow.AddMinutes(expiryInMinutes);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(customClaims),
            Expires = expirationDate,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new JwtTokenDto()
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = expirationDate            
        };

    }

}
