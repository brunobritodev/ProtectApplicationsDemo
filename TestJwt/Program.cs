using System;

namespace TestJwt
{
    class Program
    {
        static void Main(string[] args)
        {
            // ECDSA using P-256 and SHA-256
            var jwt = RsaExample.Store();

            Console.WriteLine("Copie o JWS no jwt.io");
            Console.ReadKey();

            RsaExample.Load(jwt);

            RsaExample.SavePublicJwk();
        }


    }
}
