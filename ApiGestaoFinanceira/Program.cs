using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ApiGestaoFinanceira
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseUrls(urls: "http://192.168.1.110:5000");
                });
    }
}