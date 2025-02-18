
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Project.Services;

public class TokenService
{
    private static SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));
    private static string issuer = "https://jewelry-demo.com";
    public static SecurityToken GetToken(List<Claim> claims) =>
            new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: DateTime.Now.AddDays(30.0),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

    public static TokenValidationParameters GetTokenValidationParameters() =>
        new TokenValidationParameters
        {
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero // remove delay of token when expire
        };

    public static string WriteToken(SecurityToken token) =>
        new JwtSecurityTokenHandler().WriteToken(token);

    public static int GetUserIdFromToken(string token)
    {
        token = token.Replace("Bearer ", "").Trim();
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        string userIdFromToken = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        return int.Parse(userIdFromToken);
    }

    public static string GetDetailsFromToken(string token, string detail)
    {
        token = token.Replace("Bearer ", "").Trim();
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        string detailFromToken = jwtToken.Claims.FirstOrDefault(c => c.Type == detail)?.Value;
        return detailFromToken;
    }
}