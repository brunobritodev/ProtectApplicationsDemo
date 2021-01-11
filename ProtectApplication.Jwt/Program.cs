using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtectApplication.Jwt.Data;
using System.Threading.Tasks;
using ProtectApplication.Jwt.Configuration;

namespace ProtectApplication.Jwt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            Task.WaitAll(DbMigrationHelpers.EnsureSeedData(host.Services.CreateScope()));

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

}
