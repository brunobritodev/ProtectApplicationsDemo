using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace GenerateJwt
{
    class Program
    {

        static void Main(string[] args)
        {
            /*
             * PARTE 1 - Gerar JWS
             */
            var jws = GenerateToken();

            Console.WriteLine($"{jws}{Environment.NewLine}");
            Console.WriteLine("Copie o JWS no jwt.io");
            Console.ReadKey();
        }

        private static string GenerateToken()
        {
            var now = DateTime.Now;
            var jwt = new SecurityTokenDescriptor
            {
                Issuer = "www.mysite.com",
                Audience = "your-spa",
                IssuedAt = now,
                NotBefore = now,
                Expires = now.AddHours(1),
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email, "meuemail@gmail.com", ClaimValueTypes.Email),
                    new Claim(JwtRegisteredClaimNames.GivenName, "Bruno Brito"),
                    new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
                })
            };

            // ECDSA using P-256 and SHA-256
            var jws = RsaExample.GenerateJws(jwt);

            return jws;
        }
    }
}
