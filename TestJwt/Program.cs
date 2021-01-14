using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace TestJwt
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

            /*
             * PARTE 2 - Validar JWS
             */
            // var jws = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjZmMzZmYTczLTE4ZmMtNDBmMC1iNGU1LWJmNDhmZDU5ZmQxMiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6Im1ldWVtYWlsQGdtYWlsLmNvbSIsImdpdmVuX25hbWUiOiJCcnVubyBCcml0byIsInN1YiI6ImMwMTI1MWE3LTE3YjItNGI0Ny04ODU3LTY1OWFiZDUzMzczMiIsImF1ZCI6InlvdXItc3BhIiwiZXhwIjoxNjEwNjQwNDU2LCJpc3MiOiJ3d3cubXlzaXRlLmNvbSIsImlhdCI6MTYxMDYzNjg1NiwibmJmIjoxNjEwNjM2ODU2fQ.np0fWhC2-mAvRvcrjPkAsS5yMGV2P0SB96NeWbc19ReWxBrBzwVxNYUKHI4BVsaHkHKy0m0DcO_86O5uw8Jsrlhx3xJWPnSx2qrvp_3ctgHlwol1eMnCbJ3MqgW4A48uM_4l7o_8OUvyjpR3pIEs9xjbixm7O-1IlMrvdmVrqomUngS1vVSfMD1ZgV2JyYCZV6sA77l4dYnlb-mkZHD8sLm03mshMIMYctAVg6NqXasSIiTA6m5lGtYWhfGbY8C6bmG8QR4wBHIabytVVIAe0bZJ0lBpH4fm0teBhZ47vTkMN1hBZPWKwu_22tDMNkNlIZwAhjrLO6q2_kuwHP4yKg";
            // 
            //var validationResult = RsaExample.Validate(jws);
            //Console.WriteLine("Este JWT é válido ?: {0}", validationResult.IsValid);
            //Console.WriteLine("Claims:");
            //Console.WriteLine(JsonSerializer.Serialize(validationResult.ClaimsIdentity.Claims.Select(s => new { s.Type, s.Value }), new JsonSerializerOptions() { IgnoreNullValues = true, WriteIndented = true }));
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
