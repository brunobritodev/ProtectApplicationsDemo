using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;

namespace TestJwt
{
    public class RsaExample
    {
        private static readonly DateTime Now = DateTime.Now;
        private static readonly SecurityTokenDescriptor Jwt = new SecurityTokenDescriptor
        {
            Issuer = "www.mysite.com",
            Audience = "your-spa",
            IssuedAt = Now,
            NotBefore = Now,
            Expires = Now.AddHours(1),
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, "meuemail@gmail.com", ClaimValueTypes.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, "Bruno Brito"),
                new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
            })
        };
        private static readonly TokenValidationParameters TokenValidationParams = new TokenValidationParameters
        {
            ValidIssuer = "www.mysite.com",
            ValidAudience = "your-spa",
        };


        public static string Store()
        {
            var tokenHandler = new JsonWebTokenHandler();
            var key = new RsaSecurityKey(RSA.Create(2048))
            {
                KeyId = Guid.NewGuid().ToString()
            };

            Jwt.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSsaPssSha256);
            var lastJws = tokenHandler.CreateToken(Jwt);

            Console.WriteLine($"{lastJws}{Environment.NewLine}");

            // Store in filesystem
            var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(key);

            File.WriteAllText("current-rsa.key", JsonSerializer.Serialize(jwk));

            return lastJws;
        }

        public static void Load(string jwt)
        {
            var tokenHandler = new JsonWebTokenHandler();
            var storedJwk = JsonSerializer.Deserialize<JsonWebKey>(File.ReadAllText("current-rsa.key"));
            TokenValidationParams.IssuerSigningKey = storedJwk;
            var validationResult = tokenHandler.ValidateToken(jwt, TokenValidationParams);

            Console.WriteLine(validationResult.IsValid);
        }

        public static void SavePublicJwk()
        {
            var storedJwk = JsonSerializer.Deserialize<JsonWebKey>(File.ReadAllText("current-rsa.key"));
            var pub = new PublicJsonWebKey(storedJwk);

            File.WriteAllText("public-current-rsa.key", JsonSerializer.Serialize(pub, new JsonSerializerOptions() { IgnoreNullValues = true }));
        }
    }
}