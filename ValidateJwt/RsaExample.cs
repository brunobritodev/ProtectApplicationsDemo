using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ValidateJwt
{
    public class RsaExample
    {
        private static readonly string PublicJwkFile = Path.Combine(GetApplicationRoot(), "public-tempkey.jwk");
     
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
            var dir = Path.Combine(Directory.GetParent(appRoot).FullName, "GenerateJwt");
            return dir;
        }
    }
}