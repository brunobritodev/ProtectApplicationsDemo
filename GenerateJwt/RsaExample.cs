using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace GenerateJwt
{
    public class RsaExample
    {
        private static readonly string PrivateJwkFile = Path.Combine(GetApplicationRoot(), "tempkey.jwk");
        private static readonly string PublicJwkFile = Path.Combine(GetApplicationRoot(), "public-tempkey.jwk");
        public static string GenerateJws(SecurityTokenDescriptor jwt)
        {
            var tokenHandler = new JsonWebTokenHandler();

            var rsaKey = GetKey();

            jwt.SigningCredentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);

            return tokenHandler.CreateToken(jwt);
        }

        private static JsonWebKey GetKey()
        {
            if (!File.Exists(PrivateJwkFile))
            {
                // Gerar RSA
                var newJwk = GenerateRSA();

                // Salvar a chave privada
                StorePrivateJwk(newJwk);

                // Salvar a chave pública
                SavePublicJwk(newJwk);

                return newJwk;
            }

            // Retornar a chave privada
            return GetStoredJwk();
        }

        private static JsonWebKey GetStoredJwk()
        {
            return JsonSerializer.Deserialize<JsonWebKey>(File.ReadAllText(PrivateJwkFile));
        }

        private static JsonWebKey GenerateRSA()
        {
            var key = new RsaSecurityKey(RSA.Create(2048))
            {
                KeyId = Guid.NewGuid().ToString()
            };

            var newJwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(key);
            return newJwk;
        }

        private static void StorePrivateJwk(JsonWebKey newJwk)
        {
            File.WriteAllText(PrivateJwkFile, JsonSerializer.Serialize(newJwk, new JsonSerializerOptions() { IgnoreNullValues = true, WriteIndented = true }));
        }

        public static TokenValidationResult Validate(string jwt)
        {
            var tokenValidationParams = new TokenValidationParameters
            {
                ValidIssuer = "www.mysite.com",
                ValidAudience = "your-spa",
            };

            var tokenHandler = new JsonWebTokenHandler();
            var storedJwk = JsonSerializer.Deserialize<PublicJsonWebKey>(File.ReadAllText(PublicJwkFile));
            tokenValidationParams.IssuerSigningKey = storedJwk.ToNativeJwk();
            var validationResult = tokenHandler.ValidateToken(jwt, tokenValidationParams);

            return validationResult;
        }

        public static void SavePublicJwk(JsonWebKey newJwk)
        {
            var pub = new PublicJsonWebKey(newJwk);
            File.WriteAllText(PublicJwkFile, JsonSerializer.Serialize(pub, new JsonSerializerOptions() { IgnoreNullValues = true, WriteIndented = true }));
        }

        private static string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return appRoot;
        }
    }
}